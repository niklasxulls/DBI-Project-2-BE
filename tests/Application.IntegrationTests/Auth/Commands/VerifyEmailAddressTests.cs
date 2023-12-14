using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Auth.Commands.Verify;
using stackblob.Domain.Util;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace stackblob.stackblob.Application.IntegrationTests.Auth.Commands;
public class VerifyEmailAddressTests : TestBase
{
    public VerifyEmailAddressTests(SetupFixture setup) : base(setup)
    {
    }

    [Fact]
    public async Task ShouldVerifyEmail()
    {
        var command = new RegisterUserCommand
        {
            Firstname = "Niklas",
            Lastname = "Ullsperger",
            Password = "myPassword",
            Email = "niklas.ullsperger@x-bs.at",
        };

        var userAuthDto = await SendMediator(command, explicitNonUser: true);
        var verification = _context.UserEmailVerifications.FirstOrDefault(u => u.UserId == userAuthDto.UserId);

        verification.Should().NotBeNull();
        verification!.CreatedAt.Should().BeCloseTo(DateTimeUtil.Now(), TimeSpan.FromSeconds(10));
        verification!.ExpiresAt.Should().BeAfter(DateTimeUtil.Now());
        verification.IsVerified.Should().BeFalse();
        verification.UserEmailVerificationAccess.Should().NotBeEmpty();

        var verifyCommand = new VerifyUserEmailCommand()
        {
            Guid = verification.UserEmailVerificationAccess
        };

        await SendMediator(verifyCommand, explicitNonUser: true);

        var updatedVerification = _context.UserEmailVerifications.First(v => v.UserEmailVerificationId == verification.UserEmailVerificationId);

        updatedVerification.Should().NotBeNull();
        updatedVerification.IsVerified.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldFailVerificationExpired()
    {
        var user = _context.Users.First(u => u.EmailVerficiations.Any(e => e.IsVerified));

        foreach(var verification in user.EmailVerficiations)
        {
            verification.IsVerified = false;
            verification.ExpiresAt = DateTimeUtil.Now();
        }

        var verifyCommand = new VerifyUserEmailCommand()
        {
            Guid = user.EmailVerficiations.First().UserEmailVerificationAccess
        };

        await FluentActions.Invoking(() => SendMediator(verifyCommand, explicitNonUser: true)).Should().ThrowAsync<Exception>();
    }
}
