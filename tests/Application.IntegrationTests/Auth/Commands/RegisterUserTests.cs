using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationTests;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using stackblob.Application.Exceptions;
using stackblob.Application.Models.DTOs.Users;
using stackblob.Application.UseCases.Auth.Commands.Register;
using stackblob.Application.UseCases.Comments.Commands.AddQuestion;
using stackblob.Domain.Enums;
using stackblob.Domain.Util;
using Xunit;

namespace stackblob.stackblob.Application.IntegrationTests.Auth.Commands;
public class RegisterUserTests : TestBase
{
    public RegisterUserTests(SetupFixture setup) : base(setup)
    {
    }



    [Fact]
    public async Task ShouldRegisterUser()
    {
        var userCnt = _context.Users.Count();

        var command = new RegisterUserCommand {
            Firstname = "Niklas",
            Lastname = "Ullsperger",
            Password = "myPassword",
            Email = "niklas.ullsperger@x-bs.at",
        };

        var userAuthDto = await SendMediator(command, explicitNonUser: true);

        _context.Users.Count().Should().Be(userCnt + 1);
        
        userAuthDto.Firstname.Should().Be(command.Firstname);
        userAuthDto.Lastname.Should().Be(command.Lastname);
        userAuthDto.Email.Should().Be(command.Email);
        userAuthDto.BannerUrl.Should().BeNull();
        userAuthDto.ProfilePictureUrl.Should().BeNull();
        userAuthDto.CreatedAt.Should().BeCloseTo(DateTimeUtil.Now(), TimeSpan.FromSeconds(10));
        
        var verification = _context.UserEmailVerifications.FirstOrDefault(u => u.UserId == userAuthDto.UserId);
        
        verification.Should().NotBeNull();
    }

    [Fact]
    public async Task ShouldRegisterUserWithProfilePicture()
    {
        var bytes = Encoding.UTF8.GetBytes("This is a dummy file");
        var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "dummy.txt")
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/html"
        };

        var command = new RegisterUserCommand
        {
            Firstname = "Niklas",
            Lastname = "Ullsperger",
            Password = "myPassword",
            Email = "niklas.ullsperger@x-bs.at",
            ProfilePicture = file
        };

        var userAuthDto = await SendMediator(command, explicitNonUser: true);
        
        var user = _context.Users.First(u => u.UserId == userAuthDto.UserId);

        user.ProfilePicture.Should().NotBeNull();
        (await _fileService.AttachmentExists(user.ProfilePicture!, default)).Should().BeTrue();
    }

    [Fact]
    public async Task ShouldRegisterWithSocials()
    {

        var command = new RegisterUserCommand
        {
            Firstname = "Niklas",
            Lastname = "Ullsperger",
            Password = "myPassword",
            Email = "niklas.ullsperger@x-bs.at",
            Socials = new List<SocialDto>()
            {
                new SocialDto()
                {
                    SocialTypeId = SocialType.Github,
                    Url = "https://github.com/users/niklasxulls"
                }
            }
        };

        var userAuthDto = await SendMediator(command, explicitNonUser: true);

        var user = _context.Users.First(u => u.UserId == userAuthDto.UserId);

        user.Socials.Should().HaveCount(command.Socials.Count);

        foreach(var social in command.Socials)
        {
            user.Socials.FirstOrDefault(s => s.SocialTypeId == social.SocialTypeId).Should().NotBeNull();
        }
    }

    [Fact]
    public async Task ShouldFailEmailAlreadyRegisterd()
    {
        var user = _context.Users.First();

        var command = new RegisterUserCommand
        {
            Firstname = "Niklas",
            Lastname = "Ullsperger",
            Password = "myPassword",
            Email = user.Email,
        };

        await FluentActions.Invoking(() => SendMediator(command, explicitNonUser: true)).Should().ThrowAsync<ValidationException>();
    }
}
