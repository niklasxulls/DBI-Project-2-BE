using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Questions.Queries.GetById;
using stackblob.Application.UseCases.Users.Queries.GetById;
using stackblob.Domain.Util;
using Xunit;

namespace stackblob.stackblob.Application.IntegrationTests.Users.Queries;
public class GetUserQueryTests : TestBase
{
    public GetUserQueryTests(SetupFixture setup) : base(setup)
    {
    }



    [Fact]
    public async Task ShouldGetUser()
    {
        var user = _context.Users.First();

        var cmd = new GetUserQuery()
        {
            UserId = user.UserId
        };

        var userRes = await SendMediator(cmd, explicitNonUser: true);

        userRes.Should().NotBeNull();
        userRes.UserId.Should().Be(user.UserId);
    }



    [Fact]
    public async Task ShouldFailUserDoesntExists()
    {
        var cmd = new GetUserQuery()
        {
            UserId = short.MaxValue
        };

        await FluentActions.Invoking(() => SendMediator(cmd)).Should().ThrowAsync<ValidationException>();
    }
}
