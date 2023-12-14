using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Models;
using stackblob.Application.UseCases.Answers.Queries.Get;
using stackblob.Application.UseCases.Comments.Queries.Get;
using Xunit;

namespace stackblob.stackblob.Application.IntegrationTests.Comments.Queries;
public class GetCommentsQueryTests : TestBase
{
    public GetCommentsQueryTests(SetupFixture setup) : base(setup)
    {
    
    }

    [Fact]
    public async Task ShouldGetQuestionComments()
    {
    }

    [Fact]
    public async Task ShouldGetAnswerComments()
    {
        var answer = _context.Answers.OrderByDescending(c => c.Comments.Count).First();

        var query = new GetCommentsQuery()
        {
            QuestionId = answer.QuestionId,
            AnswerId = answer.AnswerId,
            Paging = new Paging
            {
                Offset = 0,
                Size = 1000
            }
        };


        var comments = await SendMediator(query, explicitNonUser: true);

        comments.Should().NotBeNull();
        comments.Should().HaveCount(answer.Comments.Count);
    }


}
