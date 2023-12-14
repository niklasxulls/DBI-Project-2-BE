using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using stackblob.Application.Interfaces.Services;
using stackblob.Application.Interfaces;
using stackblob.Application.UseCases.Auth.Commands.Login;
using Xunit;
using stackblob.Application.UseCases.Comments.Queries.Get;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Users.Queries.GetById;

namespace stackblob.Application.UnitTests.Validators;
public class UserValidatorTests
{

    private readonly Mock<IStackblobDbContext> _context;
    private readonly Mock<ICurrentUserService> _currentUser;
    private readonly Mock<IAuthService> _auth;


    public UserValidatorTests()
    {
        _context = new Mock<IStackblobDbContext>();
        _currentUser = new Mock<ICurrentUserService>();
        _auth = new Mock<IAuthService>();
    }

    [Fact]
    public void LoginUserCommandValidator_Should_Require_Any_Fields()
    {
        var command = new LoginUserCommand();
        var validator = new LoginUserCommandValidator(_context.Object, _auth.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void RegisterUserCommandValidator_Should_Require_Any_Fields()
    {
        var command = new RegisterUserCommand();
        var validator = new RegisterUserCommandValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

    [Fact]
    public void GetUserQueryValidator_Should_Require_Any_Fields()
    {
        var command = new GetUserQuery();
        var validator = new GetUserQueryValidator(_context.Object);
        var validationResult = validator.Validate(command);
        validationResult.Errors.Should().NotBeEmpty();
    }

}
