using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Application.Models.DTOs.Questions;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Queries.GetById;
using stackblob.Application.UseCases.Questions.Queries.Search;
using stackblob.Domain.Entities;
using stackblob.Domain.Util;
using Xunit;

namespace stackblob.Application.IntegrationTests.Questions.Queries;
public class GetQuestionsQueryTests : TestBase
{
    public GetQuestionsQueryTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldPageQuestions()
    {
        var query = new GetQuestionsQuery()
        {
            Paging = new Paging
            {
                Offset = 0,
                Size = 5
            }
        };


        var questionRes = await SendMediator(query, explicitNonUser: true);

        questionRes.Should().NotBeNull();
        questionRes.Should().HaveCount(query.Paging.Size);
    }

    [Theory]
    [InlineData(SearchQuestionsOrderBy.Newest)]
    [InlineData(SearchQuestionsOrderBy.Oldest)]
    [InlineData(SearchQuestionsOrderBy.Popularity)]
    public async Task ShouldSortQuestionsTheory(SearchQuestionsOrderBy orderBy)
    {
        var query = new GetQuestionsQuery()
        {
            Paging = new Paging
            {
                Offset = 0,
                Size = 15
            }
        };

        var questionRes = (await SendMediator(query, explicitNonUser: true)).ToList();

        
        var questionQuery = _context.Questions.Take(query.Paging.Size);
        var questions = new List<Question>();

        switch (orderBy)
        {
            case SearchQuestionsOrderBy.Newest: questions = questionQuery.OrderByDescending(q => q.CreatedAt).ToList(); break;
            case SearchQuestionsOrderBy.Oldest: questions = questionQuery.OrderBy(q => q.CreatedAt).ToList(); break;
        }


        for(int i = 0; i < questions.Count; i++)
        {
            questions[i].QuestionId.Should().Equals(questionRes[i].QuestionId);
        }
    }

    [Fact]
    public async Task ShouldSearchQuestionsBySearchTerm()
    {
        var query = new GetQuestionsQuery()
        {
            SearchTerm = "C++",
            Paging = new Paging {
                Offset = 0,
                Size = 1000
            }
        };


        var questionRes = await SendMediator(query, explicitNonUser: true);
        questionRes.Should().NotBeNull();

        var questions = _context.Questions.Where(q => q.Title.Contains(query.SearchTerm)).ToList();
        questionRes.Should().HaveCount(Math.Min(questions.Count, query.Paging.Size));
    }

    [Fact]
    public async Task ShouldSearchQuestionsByTags()
    {
        var tags = _context.Tags.Select(t => t.TagId).Take(2).ToList();

        var query = new GetQuestionsQuery()
        {
            Tags = tags,
            Paging = new Paging
            {
                Offset = 0,
                Size = 1000
            }
        };


        var questionRes = await SendMediator(query, explicitNonUser: true);
        questionRes.Should().NotBeNull();
        
        var questions = _context.Questions.Where(q => q.Tags.Any(t => tags.Contains(t.TagId))).ToList();
        questionRes.Should().HaveCount(Math.Min(questions.Count, query.Paging.Size));
    }

    [Fact]
    public async Task ShouldSearchQuestionsByUser()
    {
        var query = new GetQuestionsQuery()
        {
            CreatedBy = DefaultUser.UserId,
            Paging = new Paging
            {
                Offset = 0,
                Size = 1000
            }
        };


        var questionRes = await SendMediator(query, explicitNonUser: true);
        questionRes.Should().NotBeNull();

        var questions = _context.Questions.Where(q => q.CreatedById == DefaultUser.UserId).ToList();
        questionRes.Should().HaveCount(Math.Min(questions.Count, query.Paging.Size));
    }

    [Fact]
    public async Task ShouldSearchQuestionsByDateRange()
    {
        var end = DateTimeUtil.Now();
        var start = end.AddDays(-14);

        var query = new GetQuestionsQuery()
        {
            CreatedAtFrom = DateOnly.FromDateTime(start),
            CreatedAtTill = DateOnly.FromDateTime(end),
            Paging = new Paging
            {
                Offset = 0,
                Size = 1000
            }
        };


        var questionRes = await SendMediator(query, explicitNonUser: true);
        questionRes.Should().NotBeNull();

        var questions = _context.Questions.Where(q => q.CreatedAt >= start && q.CreatedAt <= end).ToList();
        questionRes.Should().HaveCount(Math.Min(questions.Count, query.Paging.Size));
    }




}
