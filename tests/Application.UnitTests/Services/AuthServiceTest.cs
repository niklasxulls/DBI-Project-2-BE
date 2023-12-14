
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using Moq;
//using Rest;
//using stackblob.Application.Exceptions;
//using stackblob.Application.Interfaces;
//using stackblob.Application.Interfaces.Services;
//using stackblob.Application.Mapping;
//using stackblob.Application.UseCases.Auth.Commands;
//using stackblob.Application.UseCases.Auth.Commands.Login;
//using stackblob.Domain.Entities;
//using stackblob.Domain.Settings;
//using stackblob.Infrastructure.Services;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Application.UnitTests.Services;

//public class AuthServiceFixture : IDisposable
//{
//    public IAuthService AuthService { get; private set; }
//    //public AuthSettings AuthSettings { get; private set; }
//    public Mock<IStackblobDbContext> Context { get; private set; }
//    private readonly Mock<ICurrentUserService> _userService = new Mock<ICurrentUserService>();
//    private readonly Mock<IPAddressResolverService> _ipAddressResolverService = new Mock<IPAddressResolverService>();
//    //public Mock<IMapper> Mapper { get; private set; }
//    public User CorrectUser { get; private set; }
//    public string HashedPassword { get; private set; }
//    public AuthServiceFixture()
//    {
//        var builder = new ConfigurationBuilder()
//                   .SetBasePath(Directory.GetCurrentDirectory())
//                   .AddJsonFile("appsettings.Test.json", true, true)
//                   .AddEnvironmentVariables();


//        var _configuration = builder.Build();

//        var startup = new Startup(_configuration);

//        var services = new ServiceCollection();

//        var authSection = _configuration.GetSection("AuthSettings");
//        services.Configure<AuthSettings>(authSection);

//        var authSettings = authSection.Get<AuthSettings>();

//        Context = new Mock<IStackblobDbContext>();
//        var tokenValidationParams = Options.Create(_configuration.Get<TokenValidationParameters>());

//        var profile = new MappingProfile();
//        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
//        var mapper = new Mapper(configuration);


//        AuthService = new AuthService(Options.Create(authSettings), Context.Object, _userService.Object, _ipAddressResolverService.Object, mapper,   _configuration.Get<TokenValidationParameters>());

//        var dataSource = new List<User>(); //<-- this will hold data
//        var user = new User()
//        {
//            UserId = 1,
//            Firstname = "Test",
//            Lastname = "Test",
//            Email = "test@test.com",
//            Password = "Test",
//            Salt = AuthService.CreateSalt()
//        };
//        HashedPassword = user.Salt + user.Password;
//        CorrectUser = (User)CloneObj.Clone(user);
//        user.Password = AuthService.CreateHash(user.Salt + user.Password);
//        dataSource.Add(user);

//        var mockSet = new MockDbSet<User>(dataSource);

//        Context.Setup(x => x.Users).Returns(mockSet.Object);

//    }

//    public void Dispose()
//    {
//    }
//}
//public class AuthServiceTest : IClassFixture<AuthServiceFixture>
//{
//    private readonly AuthServiceFixture _sut;

//    public AuthServiceTest(AuthServiceFixture sut)
//    {
//        _sut = sut;
//    }


//    [Fact]
//    public async Task AuthentificateEmailNotInDb()
//    {
//        var login = new LoginUserCommand { Email = "testwrong@test.com", Password = "xxx" };
//        await Assert.ThrowsAsync<NotFoundException>(() => _sut.AuthService.Authenticate(login));
//    }

//    [Fact]
//    public async Task AuthentificatePasswordWrong()
//    {
//        var login = new LoginUserCommand { Email = _sut.CorrectUser.Email, Password = "xxx" };
//        await Assert.ThrowsAsync<ForbiddenAccessException>(() => _sut.AuthService.Authenticate(login));
//    }

//}
