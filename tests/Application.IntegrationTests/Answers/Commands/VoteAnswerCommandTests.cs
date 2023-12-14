using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Answers.Commands.Votes;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;


namespace stackblob.Application.IntegrationTests.Answers.Commands;

public class VoteAnswerCommandTests : TestBase
{
    public VoteAnswerCommandTests(SetupFixture setup) : base(setup)
    {
    }


    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ShouldVoteAnswer(bool isUpVote)
    {
        var answer = _context.Answers.Where(u => u.CreatedById == DefaultUser.UserId).First();
        var voteCnt = answer.AnswerVotes.Count;

        VoteAnswerCommand cmd = new VoteAnswerCommand
        {
            AnswerId = answer.AnswerId,
            QuestionId = answer.QuestionId,
            IsUpVote = isUpVote,
        };

        await SendMediator(cmd);

        var updatedAnswer = _context.Answers.Where(u => u.AnswerId == answer.AnswerId).First();
        updatedAnswer.AnswerVotes.Should().HaveCount(voteCnt + 1);
        updatedAnswer.AnswerVotes.First(q => q.CreateByInAnswerId == DefaultUser.UserId && q.IsUpVote == isUpVote).Should().NotBeNull();
    }

}
