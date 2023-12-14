using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.UseCases.Questions.Commands.AcceptAnswer;
using stackblob.Application.UseCases.Questions.Commands.AddAttachment;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Application.UseCases.Questions.Commands.RemoveAttachment;
using stackblob.Application.UseCases.Questions.Commands.RemoveQuestion;
using stackblob.Application.UseCases.Questions.Commands.RevertAcceptAnswer;
using stackblob.Application.UseCases.Questions.Commands.Update;
using stackblob.Application.UseCases.Questions.Commands.UpdateQuestion;
using stackblob.Application.UseCases.Questions.Commands.Votes;
using stackblob.Application.UseCases.Questions.Queries.GetById;
using stackblob.Application.UseCases.Questions.Queries.Search;
using stackblob.Domain.Entities;
using stackblob.Infrastructure.Persistence.Configurations;
using Xunit;

namespace Application.UnitTests.Validators;
public class QuestionValidatorsTest
{
    private readonly Mock<IStackblobDbContext> _context;
    private readonly Mock<ICurrentUserService> _currentUser;



    public QuestionValidatorsTest()
    {
        _context = new Mock<IStackblobDbContext>();
        _currentUser = new Mock<ICurrentUserService>();
    }

    [Fact]
    public void AddQuestionCommandValidator_Should_Match_Configuration()
    {
        var validator = new AddQuestionCommandValidator(_context.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Question, QuestionConfiguration,
                                                          AddQuestionCommandValidator, AddQuestionCommand>(validator);
    }

    [Fact]
    public void AddQuestionCommandValidator_Should_Require_Any_Fields()
    {
        var command = new AddQuestionCommand();
        var validator = new AddQuestionCommandValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void UpdateQuestionCommandValidator_Should_Match_Configuration()
    {
        var validator = new UpdateQuestionCommandValidator(_context.Object, _currentUser.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<Question, QuestionConfiguration,
                                                          UpdateQuestionCommandValidator, UpdateQuestionCommand>(validator);
    }

    [Fact]
    public void UpdateQuestionCommandValidator_Should_Require_Any_Fields()
    {
        var command = new UpdateQuestionCommand();
        var validator = new UpdateQuestionCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void AcceptAnswerCommandValidator_Should_Require_Any_Fields()
    {
        var command = new AcceptAnswerCommand();
        var validator = new AcceptAnswerCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddQuestionAttachmentCommandValidator_Should_Require_Any_Fields()
    {
        var command = new AddQuestionAttachmentsCommand();
        var validator = new AddQuestionAttachmentsValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void RemoveQuestionCommandValidator_Should_Require_Any_Fields()
    {
        var command = new RemoveQuestionCommand();
        var validator =  new RemoveQuestionCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void RemoveQuestionAttachmentsCommandValidator_Should_Require_Any_Fields()
    {
        var command = new RemoveQuestionAttachmentsCommand();
        var validator = new RemoveQuestionAttachmentsCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void RevertAcceptAnswersCommandValidator_Should_Require_Any_Fields()
    {
        var command = new RevertAcceptAnswerCommand();
        var validator = new RevertAcceptAnswerCommandValidator(_context.Object, _currentUser.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void VoteQuestionCommandValidator_Should_Require_Any_Fields()
    {
        var command = new VoteQuestionCommand();
        var validator = new VoteQuestionCommandValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void GetQuestionQueryValidator_Should_Require_Any_Fields()
    {
        var command = new GetQuestionQuery();
        var validator = new GetQuestionQueryValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void GetQuestionsQueryValidator_Should_Require_Any_Fields()
    {
        var command = new GetQuestionsQuery();
        var validator = new GetQuestionsQueryValidator();
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

}
