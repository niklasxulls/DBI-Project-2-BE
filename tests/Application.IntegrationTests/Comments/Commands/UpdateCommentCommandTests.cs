using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Comments.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Domain.Util;
using Xunit;

namespace stackblob.Application.IntegrationTests.Comments.Commands;

public class UpdateCommentCommandTests : TestBase
{
    public UpdateCommentCommandTests(SetupFixture setup) : base(setup)
    {
    }

  

    [Fact]
    public async Task ShouldUpdateComment()
    {
        var comment = _context.Comments.First(u => u.CreatedByInQuestionId == DefaultUser.UserId);

        UpdateCommentCommand cmd = new UpdateCommentCommand
        {
            CommentId = comment.CommentId,
            Description = "Updated comment"
        };

        var commentDto = await SendMediator(cmd);
        commentDto.Should().NotBeNull();

        var updatedComment = _context.Comments.Where(c => c.CommentId == comment.CommentId).FirstOrDefault();

        commentDto!.Description.Should().Be(cmd.Description);
        comment.UpdatedAt.Should().BeCloseTo(DateTimeUtil.Now(), TimeSpan.FromSeconds(10));
    }

    [Fact]
    public async Task ShouldFailAuth()
    {
        var comment = _context.Comments.First(q => q.CreatedByInAnswerId != DefaultUser.UserId && q.CreatedByInQuestionId != DefaultUser.UserId);

        UpdateCommentCommand cmd = new UpdateCommentCommand
        {
            CommentId = comment.CommentId,
            Description = "Content of my Test Question"
        };
        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
