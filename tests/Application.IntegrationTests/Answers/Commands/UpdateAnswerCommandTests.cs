using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using Xunit;

namespace stackblob.Application.IntegrationTests.Answers.Commands;

public class UpdateAnswerCommandTests : TestBase
{
    public UpdateAnswerCommandTests(SetupFixture setup) : base(setup)
    {
    }



    [Fact]
    public async Task ShouldUpdateQuestion()
    {
        var answer = _context.Answers.First(u => u.CreatedById == DefaultUser.UserId);

        UpdateAnswerCommand cmd = new UpdateAnswerCommand
        {
            QuestionId = answer.QuestionId,
            AnswerId = answer.AnswerId,
            Title = "Test Answer",
            Description = "Content of my Test Answer"
        };

        var answerDto = await SendMediator(cmd);

        var updatedAnswer = _context.Answers.Where(u => u.QuestionId == answer.QuestionId && u.AnswerId == answer.AnswerId).FirstOrDefault();

        updatedAnswer.Should().NotBeNull();
        updatedAnswer!.Title.Should().Be(cmd.Title);
        updatedAnswer!.Description.Should().Be(cmd.Description);
    }

    [Fact]
    public async Task ShouldFailAuthDuringUpdateAnswer()
    {
        var answer = _context.Answers.First(u => u.CreatedById != DefaultUser.UserId);

        UpdateAnswerCommand cmd = new UpdateAnswerCommand
        {
            QuestionId = answer.QuestionId,
            AnswerId = answer.AnswerId,
            Title = "Test Question",
            Description = "Content of my Test Question"
        };
        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
