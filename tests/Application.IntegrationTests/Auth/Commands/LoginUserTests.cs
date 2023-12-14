using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Auth.Commands.Login;
using stackblob.Application.UseCases.Auth.Commands.Register;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace stackblob.stackblob.Application.IntegrationTests.Auth.Commands;
public class LoginUserTests : TestBase
{
    public LoginUserTests(SetupFixture setup) : base(setup)
    {
    }


    [Fact]
    public async Task ShouldLogin()
    {
        var user = _context.Users.First();
        var loginLocationsCnt = user.LoginLocations.Count();
        var plainPassword = UserPasswords[user.UserId];

        var cmd = new LoginUserCommand()
        {
            Email = user.Email,
            Password = plainPassword
        };

        var authDto = await SendMediator(cmd, explicitNonUser: true);
        

        authDto.Should().NotBeNull();
        authDto.Email.Should().Be(user.Email);
        authDto.RefreshToken.Should().HaveLength(64);
        authDto.AccessToken.Should().NotBeNullOrEmpty();

        _context.LoginLocations.Where(u => u.UserId == user.UserId).Should().HaveCount(loginLocationsCnt + 1);
    }


    [Fact]
    public async Task ShouldNotFindUser()
    {
        var cmd = new LoginUserCommand()
        {
            Email = "invalid@email.at",
            Password = "test"
        };
        
        await FluentActions.Invoking(() => SendMediator(cmd, explicitNonUser: true)).Should().ThrowAsync<NotFoundException>();

    }

}
