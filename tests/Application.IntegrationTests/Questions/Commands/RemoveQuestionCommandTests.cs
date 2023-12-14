using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;


namespace stackblob.Application.IntegrationTests.Questions.Commands;

public class RemoveQuestionCommandTests : TestBase
{
    public RemoveQuestionCommandTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldRemoveQuestion()
    {
        var question = _context.Questions.First(u => u.CreatedById == DefaultUser.UserId);

        RemoveQuestionCommand cmd = new RemoveQuestionCommand
        {
            QuestionId = question.QuestionId
        };

        await SendMediator(cmd);

        _context.Questions.Where(u => u.QuestionId == question.QuestionId).FirstOrDefault().Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailAuthDuringRemoveQuestion()
    {
        var question = _context.Questions.First(u => u.CreatedById != DefaultUser.UserId);

        RemoveQuestionCommand cmd = new RemoveQuestionCommand
        {
            QuestionId = question.QuestionId
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
