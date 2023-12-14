using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using Xunit;

namespace stackblob.Application.IntegrationTests.Questions.Commands;

public class UpdateQuestionCommandTests : TestBase
{
    public UpdateQuestionCommandTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldUpdateQuestion()
    {
        var question = _context.Questions.First(u => u.CreatedById == DefaultUser.UserId);

        UpdateQuestionCommand cmd = new UpdateQuestionCommand
        {
            QuestionId = question.QuestionId,
            Title = "Test Question",
            Description = "Content of my Test Question"
        };

        var questionDto = await SendMediator(cmd);

        var updatedQuestion = _context.Questions.Where(u => u.QuestionId == question.QuestionId).FirstOrDefault();

        updatedQuestion.Should().NotBeNull();
        updatedQuestion!.Title.Should().Be(cmd.Title);
        updatedQuestion!.Description.Should().Be(cmd.Description);
    }

    [Fact]
    public async Task ShouldFailAuthDuringUpdateQuestion()
    {
        var question = _context.Questions.First(u => u.CreatedById != DefaultUser.UserId);

        UpdateQuestionCommand cmd = new UpdateQuestionCommand
        {
            QuestionId = question.QuestionId,
            Title = "Test Question",
            Description = "Content of my Test Question"
        };
        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
