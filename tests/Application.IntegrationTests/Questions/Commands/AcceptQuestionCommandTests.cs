using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AcceptAnswer;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using Xunit;

namespace stackblob.Application.IntegrationTests.Questions.Commands;
public class AcceptQuestionCommandTests : TestBase
{
    public AcceptQuestionCommandTests(SetupFixture setup) : base(setup)
    {
    }

  

    [Fact]
    public async Task ShouldAcceptAnswer()
    {
        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId);
        var correctAnswer = question.Answers.First();

        var cmd = new AcceptAnswerCommand()
        {
            QuestionId = question.QuestionId,
            AnswerId = correctAnswer.AnswerId
        };

        await SendMediator(cmd);

        var updatedQuestion = _context.Questions.First(q => q.QuestionId == question.QuestionId);

        updatedQuestion.CorrectAnswerId.Should().Be(correctAnswer.AnswerId);
    }

    [Fact]
    public async Task ShoulFailDueToAuth()
    {
        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId);
        var correctAnswer = _context.Answers.First(a => a.QuestionId != question.QuestionId);

        var cmd = new AcceptAnswerCommand()
        {
            QuestionId = question.QuestionId,
            AnswerId = correctAnswer.AnswerId
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ShoulFailDueToAlreadyTaggedCorrectQuestion()
    {
        var question = _context.Questions.First(q => q.CreatedById == DefaultUser.UserId && q.CorrectAnswer == null);
        var correctAnswer = _context.Answers.First(a => a.QuestionId == question.QuestionId);

        var cmd = new AcceptAnswerCommand()
        {
            QuestionId = question.QuestionId,
            AnswerId = correctAnswer.AnswerId
        };

        await SendMediator(cmd);

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }


}
