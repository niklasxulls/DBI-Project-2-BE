using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using stackblob.Application.Exceptions;
using stackblob.Application.UseCases.Users.Commands.Remove;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace stackblob.stackblob.Application.IntegrationTests.Users.Commands;
public class RemoveUserCommandTests : TestBase
{
    public RemoveUserCommandTests(SetupFixture setup) : base(setup)
    {
    }

    [Fact]
    public async Task ShouldRemoveUser()
    {
        var user = _context.Users.Where(u => u.Question.Count() > 0 && u.Answers.Count() > 0 && u.AnswerComments.Count() > 0 && u.QuestionComments.Count() > 0 && u.UserId != DefaultUser.UserId).FirstOrDefault();
        var cmd = new RemoveUserCommand() { };
        var userCnt = _context.Users.Count();

        await SendMediator(cmd, user);

        _context.Users.Count().Should().Be(userCnt -1);
    }



    [Fact]
    public async Task ShouldFailAuth()
    {
        var cmd = new RemoveUserCommand() { };

        await SendMediator(cmd);
    }
}
