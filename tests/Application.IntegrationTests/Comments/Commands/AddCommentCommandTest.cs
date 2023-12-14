using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Comments.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using stackblob.Domain.Util;
using Xunit;


namespace stackblob.Application.IntegrationTests.Comments.Commands;

public class AddCommentCommandTest : TestBase
{
    public AddCommentCommandTest(SetupFixture setup) : base(setup)
    {
      
    }

    [Fact]
    public async Task ShouldAddComment()
    {
        var answer = _context.Answers.First();
        var commentsCnt = answer.Comments.Count;

        AddCommentCommand cmd = new AddCommentCommand
        {
            QuestionId = answer.QuestionId,
            AnswerId = answer.AnswerId,
            Description = "Content of my test comment",
        };

        var commentDto = await SendMediator(cmd);

        var comment = _context.Comments.Where(c => c.CommentId == commentDto.CommentId).FirstOrDefault();

        comment.Should().NotBeNull();

        commentDto!.QuestionId.Should().Be(comment!.QuestionId);
        commentDto!.AnswerId.Should().Be(comment!.AnswerId);
        commentDto!.Description.Should().Be(comment!.Description);
        commentDto!.CreatedBy!.UserId.Should().Be(DefaultUser.UserId);
        commentDto.CreatedAt.Should().BeCloseTo(DateTimeUtil.Now(), TimeSpan.FromSeconds(10));
    }



}
