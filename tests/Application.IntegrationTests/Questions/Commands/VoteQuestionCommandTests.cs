using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;


namespace stackblob.Application.IntegrationTests.Questions.Commands;

public class VoteQuestionCommandTests : TestBase
{
    public VoteQuestionCommandTests(SetupFixture setup) : base(setup)
    {
    }

    [Fact]
    public async Task ShouldRequireAnyFields()
    {
        var command = new VoteQuestionCommand();
        await FluentActions.Invoking(() => SendMediator(command)).Should().ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldVoteQuestion(bool isUpVote)
    {
        var question = _context.Questions.Where(u => u.CreatedById == DefaultUser.UserId).First();
        var voteCnt = question.QuestionVotes.Count;

        VoteQuestionCommand cmd = new VoteQuestionCommand
        {
            QuestionId = question.QuestionId,
            IsUpVote = isUpVote,
        };

        await SendMediator(cmd);

        var updatedQuestion = _context.Questions.Where(u => u.QuestionId == question.QuestionId).First();
        updatedQuestion.QuestionVotes.Should().HaveCount(voteCnt + 1);
        updatedQuestion.QuestionVotes.First(q => q.CreateByInQuestionId == DefaultUser.UserId && q.IsUpVote == isUpVote).Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldFailQuestionDoesntExist()
    {
        VoteQuestionCommand cmd = new VoteQuestionCommand
        {
            QuestionId = short.MaxValue
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
