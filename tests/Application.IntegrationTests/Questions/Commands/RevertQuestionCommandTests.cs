using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AcceptAnswer;
using stackblob.Application.UseCases.Questions.Commands.RevertAcceptAnswer;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using Xunit;

namespace stackblob.Application.IntegrationTests.Questions.Commands;
public class RevertQuestionCommandTests : TestBase
{
    public RevertQuestionCommandTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldRevertAcceptAnswer()
    {
        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId && q.CorrectAnswer != null);

        var cmd = new RevertAcceptAnswerCommand()
        {
            QuestionId = question.QuestionId,
        };

        await SendMediator(cmd);

        var updatedQuestion = _context.Questions.First(q => q.QuestionId == question.QuestionId);

        updatedQuestion.CorrectAnswerId.Should().BeNull();
    }

    [Fact]
    public async Task ShoulFailDueToAuth()
    {
        var question = _context.Questions.First(q => q.CreatedById != DefaultUser.UserId);

        var cmd = new RevertAcceptAnswerCommand()
        {
            QuestionId = question.QuestionId,
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShoulFailDueToNonExistingQuestion()
    {
        var cmd = new RevertAcceptAnswerCommand()
        {
            QuestionId = short.MaxValue,
        };


        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }


}
