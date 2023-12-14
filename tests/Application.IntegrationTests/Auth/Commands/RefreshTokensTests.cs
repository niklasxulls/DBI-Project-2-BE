using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Auth.Commands.Login;
using stackblob.Application.UseCases.Auth.Commands.RefreshTokens;
using stackblob.Domain.Util;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace stackblob.stackblob.Application.IntegrationTests.Auth.Commands;
public class RefreshTokensTests : TestBase
{
    public RefreshTokensTests(SetupFixture setup) : base(setup)
    {
    }

    [Fact]
    public async Task ShouldRefreshTokens()
    {
        var user = _context.Users.First();
        var plainPassword = UserPasswords[user.UserId];

        var loginCmd = new LoginUserCommand()
        {
            Email = user.Email,
            Password = plainPassword
        };

        var authUserDto = await SendMediator(loginCmd, explicitNonUser: true);

        var refreshTokensCmd = new RefreshTokensCommand()
        {
            AccessToken = authUserDto.AccessToken,
            RefreshToken = authUserDto.RefreshToken,
        };

        var newTokens = await SendMediator(refreshTokensCmd);

        newTokens.Should().NotBeNull();
        newTokens.RefreshToken.Should().HaveLength(64);
        newTokens.AccessToken.Should().NotBeNullOrEmpty();
        newTokens.ExpiresAt.Should().BeAfter(DateTimeUtil.Now());
    }

    [Fact]
    public async Task ShouldFailInvalidTokens()
    {
        var refreshTokensCmd = new RefreshTokensCommand()
        {
            AccessToken = "thisIsAnInValidAccessToken",
            RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9eyJVc2VySWQiOeyfiafdsefasdfc",
        };

        await FluentActions.Invoking(() => SendMediator(refreshTokensCmd, explicitNonUser: true)).Should().ThrowAsync<ForbiddenAccessException>();
    }


    [Fact]
    public async Task ShouldFailExpiredRefreshToken()
    {
        var user = _context.Users.First();
        var plainPassword = UserPasswords[user.UserId];

        var loginCmd = new LoginUserCommand()
        {
            Email = user.Email,
            Password = plainPassword
        };

        var authUserDto = await SendMediator(loginCmd, explicitNonUser: true);

        var refreshToken = _context.RefreshTokens.First(r => r.Token == authUserDto.RefreshToken);
        refreshToken.ExpiresAt = DateTimeUtil.Now().AddMinutes(-5);
        _context.SaveChanges();

        var refreshTokensCmd = new RefreshTokensCommand()
        {
            AccessToken = authUserDto.AccessToken,
            RefreshToken = authUserDto.RefreshToken,
        };


        await FluentActions.Invoking(() => SendMediator(refreshTokensCmd, explicitNonUser: true)).Should().ThrowAsync<ForbiddenAccessException>();
    }
}
