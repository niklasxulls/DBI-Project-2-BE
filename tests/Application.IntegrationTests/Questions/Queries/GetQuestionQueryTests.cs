using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Queries.GetById;
using stackblob.Domain.Util;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace stackblob.Application.IntegrationTests.Questions.Queries;
public class GetQuestionQueryTests : TestBase
{
    public GetQuestionQueryTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldGetQuestion()
    {
        var question = _context.Questions.First();

        var cmd = new GetQuestionQuery()
        {
            QuestionIdAccess = question.QuestionIdAccess
        };


        var questionRes = await SendMediator(cmd, explicitNonUser: true);

        questionRes.Should().NotBeNull();
        questionRes.QuestionId.Should().Be(question.QuestionId);
    }



    [Fact]
    public async Task ShoulFailQuestionDoesntExist()
    {
        var cmd = new GetQuestionQuery()
        {
            QuestionIdAccess = GuidUtil.NewGuid()
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }


}
