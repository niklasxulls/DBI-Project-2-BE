using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UnitTests.Validators;
using FluentAssertions;
using Moq;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Comments.Commands.AddQuestion;
using stackblob.Application.UseCases.Comments.Queries.Get;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Domain.Entities;
using stackblob.Infrastructure.Persistence.Configurations;
using Xunit;

namespace stackblob.Application.UnitTests.Validators;

public class CommentValidatorTests
{
    private readonly Mock<IStackblobDbContext> _context;
    private readonly Mock<ICurrentUserService> _currentUser;


    public CommentValidatorTests()
    {
        _context = new Mock<IStackblobDbContext>();
        _currentUser = new Mock<ICurrentUserService>();
    }

    [Fact]
    public void AddCommentCommandValidator_Should_Match_Configuration()
    {
        var validator = new AddCommentCommandValidator(_context.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Comment, CommentConfiguration, AddCommentCommandValidator, AddCommentCommand>(validator);
    }

    [Fact]
    public void AddCommentCommandValidator_Should_Require_Any_Fields()
    {
        var command = new AddCommentCommand();
        var validator = new AddCommentCommandValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }


    [Fact]
    public void UpdateCommentCommandValidator_Should_Match_Configuration()
    {
        var validator = new UpdateCommentCommandValidator(_context.Object, _currentUser.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Comment, CommentConfiguration, UpdateCommentCommandValidator, UpdateCommentCommand>(validator);
    }

    [Fact]
    public void UpdateCommentCommandValidator_Should_Require_Any_Fields()
    {
        var command = new UpdateCommentCommand();
        var validator = new UpdateCommentCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void GetCommentQueryValidator_Should_Match_Configuration()
    {
        var validator = new GetCommentsQueryValidator();
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Comment, CommentConfiguration, GetCommentsQueryValidator, GetCommentsQuery>(validator);
    }

    [Fact]
    public void GetCommentQueryValidator_Should_Require_Any_Fields()
    {
        var command = new GetCommentsQuery();
        var validator = new GetCommentsQueryValidator();
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }
}
