using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Models;
using stackblob.Application.UseCases.Answers.Queries.Get;
using stackblob.Application.UseCases.Questions.Queries.Search;
using stackblob.Domain.Entities;
using Xunit;

namespace stackblob.Application.IntegrationTests.Answers.Queries;


public class GetAnswersQueryTests : TestBase
{
    public GetAnswersQueryTests(SetupFixture setup) : base(setup)
    {
    }



    [Fact]
    public async Task ShouldPageAnswers()
    {
        var question = _context.Questions.OrderByDescending(q => q.Answers.Count).First();

        var query = new GetAnswersQuery()
        {
            QuestionId = question.QuestionId,
            Paging = new Paging
            {
                Offset = 0,
                Size = question.Answers.Count -1
            }
        };


        var questionRes = await SendMediator(query, explicitNonUser: true);

        questionRes.Should().NotBeNull();
        questionRes.Should().HaveCount(query.Paging.Size);
    }

    [Theory]
    [InlineData(GetAnswersOrderBy.Newest)]
    [InlineData(GetAnswersOrderBy.Oldest)]
    [InlineData(GetAnswersOrderBy.Popularity)]
    public async Task ShouldSortAnswers(GetAnswersOrderBy orderBy)
    {
        var question = _context.Questions.OrderByDescending(q => q.Answers.Count()).First();

        var query = new GetAnswersQuery()
        {
            QuestionId = question.QuestionId,
            Paging = new Paging
            {
                Offset = 0,
                Size = 15
            }
        };

        var answerRes = (await SendMediator(query, explicitNonUser: true)).ToList();


        var answerQuery = _context.Answers.Where(q => q.QuestionId == question.QuestionId).Take(query.Paging.Size);

        switch (orderBy)
        {
            case GetAnswersOrderBy.Newest: answerQuery = answerQuery.OrderByDescending(q => q.CreatedAt); break;
            case GetAnswersOrderBy.Oldest: answerQuery = answerQuery.OrderBy(q => q.CreatedAt); break;
            case GetAnswersOrderBy.Popularity: answerQuery = answerQuery.OrderByDescending(q => q.AnswerVotes.Where(v => v.IsUpVote).Count()); break;
        }

        var answers = answerQuery.ToList();

        for (int i = 0; i < answers.Count; i++)
        {
            answers[i].AnswerId.Should().Equals(answerRes[i].AnswerId);
        }
    }

    [Fact]
    public async Task ShouldGetAnswers()
    {
        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId);
        
        var query = new GetAnswersQuery()
        {
            QuestionId = question.QuestionId,
            Paging = new Paging
            {
                Offset = 0,
                Size = 1000
            }
        };


        var answerRes = await SendMediator(query, explicitNonUser: true);
        answerRes.Should().NotBeNull();

        var answers = _context.Answers.Where(a => a.QuestionId == query.QuestionId).ToList();
        answerRes.Should().HaveCount(Math.Min(answers.Count, query.Paging.Size));
    }




}