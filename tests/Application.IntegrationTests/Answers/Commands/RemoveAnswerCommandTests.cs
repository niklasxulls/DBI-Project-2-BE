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


namespace stackblob.Application.IntegrationTests.Answers.Commands;

public class RemoveAnswerCommandTests : TestBase
{
    public RemoveAnswerCommandTests(SetupFixture setup) : base(setup)
    {
    }



    [Fact]
    public async Task ShouldRemoveAnswer()
    {
        var question = _context.Questions.First(q => q.Answers.Any(a => a.CreatedById == DefaultUser.UserId));
        var answer = question.Answers.First(a => a.CreatedById == DefaultUser.UserId);

        RemoveAnswerCommand cmd = new RemoveAnswerCommand
        {
            QuestionId = question.QuestionId,
            AnswerId = answer.AnswerId
        };

        await SendMediator(cmd);

        _context.Answers.Where(u => u.QuestionId == question.QuestionId && u.AnswerId == answer.AnswerId).FirstOrDefault().Should().BeNull();
    }

    [Fact]
    public async Task ShouldFailAuthDuringRemoveAnswer()
    {
        var question = _context.Questions.First(q => q.Answers.Any(a => a.CreatedById != DefaultUser.UserId));
        var answer = question.Answers.First(a => a.CreatedById != DefaultUser.UserId);

        RemoveAnswerCommand cmd = new RemoveAnswerCommand
        {
            QuestionId = question.QuestionId,
            AnswerId = answer.AnswerId
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
