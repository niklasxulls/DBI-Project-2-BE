using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UnitTests.Validators;
using FluentAssertions;
using Moq;
using stackblob.Application.Exceptions;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Answers.Commands.Votes;
using stackblob.Application.UseCases.Answers.Queries.Get;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Domain.Entities;
using stackblob.Infrastructure.Persistence.Configurations;
using Xunit;

namespace stackblob.Application.UnitTests.Validators;

public class AnswerValidatorTests
{
    private readonly Mock<IStackblobDbContext> _context;
    private readonly Mock<ICurrentUserService> _currentUser;


    public AnswerValidatorTests()
    {
        _context = new Mock<IStackblobDbContext>();
        _currentUser = new Mock<ICurrentUserService>();
    }

    [Fact]
    public void AddAnswerCommandValidator_Should_Match_Configuration()
    {
        var validator = new AddAnswerCommandValidator(_context.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Answer, AnswerConfiguration, AddAnswerCommandValidator, AddAnswerCommand>(validator);
    }

    [Fact]
    public void AddAnswerCommandValidator_Should_Require_Any_Fields()
    {
        var command = new AddAnswerCommand();
        var validator = new AddAnswerCommandValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }


    [Fact]
    public void UpdateAnswerCommandValidator_Should_Match_Configuration()
    {
        var validator = new UpdateAnswerCommandValidator(_context.Object, _currentUser.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Answer, AnswerConfiguration, UpdateAnswerCommandValidator, UpdateAnswerCommand>(validator);
    }

    [Fact]
    public void UpdateAnswerCommandValidator_Should_Require_Any_Fields()
    {
        var command = new UpdateAnswerCommand();
        var validator = new UpdateAnswerCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void RemoveAnswerCommandValidator_Should_Require_Any_Fields()
    {
        var command = new RemoveAnswerCommand();
        var validator = new RemoveAnswerCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAnswerQueryValidator_Should_Require_Any_Fields()
    {
        var command = new GetAnswersQuery();
        var validator = new GetAnswersQueryValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void VoteAnswersCommandValidator_Should_Require_Any_Fields()
    {
        var command = new VoteAnswerCommand();
        var validator = new VoteAnswerCommandValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

}
