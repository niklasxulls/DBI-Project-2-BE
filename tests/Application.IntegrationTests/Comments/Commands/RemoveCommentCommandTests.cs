using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Comments.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Domain.Entities;
using stackblob.Domain.Enums;
using Xunit;


namespace stackblob.Application.IntegrationTests.Comments.Commands;

public class RemoveCommentCommandTests : TestBase
{
    public RemoveCommentCommandTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldRemoveComment()
    {
        var comment = _context.Comments.First(q => q.CreatedByInQuestionId == DefaultUser.UserId);
        var commentCnt = _context.Comments.Count();

        RemoveCommentCommand cmd = new RemoveCommentCommand
        {
            CommentId = comment.CommentId
        };

        await SendMediator(cmd);

        _context.Comments.Where(c => c.CommentId == comment.CommentId).FirstOrDefault().Should().BeNull();
        _context.Comments.Count().Should().Be(commentCnt - 1);
    }

    [Fact]
    public async Task ShouldFailAuth()
    {
        var comment = _context.Comments.First(q => q.CreatedByInAnswerId != DefaultUser.UserId && q.CreatedByInQuestionId != DefaultUser.UserId);


        RemoveCommentCommand cmd = new RemoveCommentCommand
        {
            CommentId = comment.CommentId
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
