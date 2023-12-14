using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.UnitTests.Validators;
using Moq;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Questions.Commands.AddQuestion;
using stackblob.Domain.Entities;
using stackblob.Infrastructure.Persistence.Configurations;
using Xunit;
using stackblob.Application.UseCases.Auth.Commands.Register;
using FluentAssertions;

namespace stackblob.Application.UnitTests.Validators;
public class RegisterValidatorTests
{
    private readonly Mock<IStackblobDbContext> _context;
    private readonly Mock<ICurrentUserService> _currentUser;

    public RegisterValidatorTests()
    {
        _context = new Mock<IStackblobDbContext>();
        _currentUser = new Mock<ICurrentUserService>();
    }

    [Fact]
    public void RegisterUserCommandValidator_Should_Match_Configuration()
    {
        var validator = new RegisterUserCommandValidator(_context.Object);
        ValidatorTestUtil.CheckValidatorWithEntityTypeBuilder<User, UserConfiguration,
                                                          RegisterUserCommandValidator, RegisterUserCommand>(validator);
    }

    [Fact]
    public void RegisterUserCommandValidator_Should_Fail_Invalid_Email()
    {
        var validator = new RegisterUserCommandValidator(_context.Object);
        var validationResult = validator.Validate(new RegisterUserCommand()
        {
            Firstname = "Test",
            Lastname = "Test",
            Password = "Test",
            Email = "invalidEmail"
        });
        validationResult.Errors.Should().HaveCount(1);
    }
}
