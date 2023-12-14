using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rest;
using Moq;
using stackblob.Infrastructure.Persistence;
using System.Reflection;
using stackblob.Application.Interfaces;
using Xunit;
using MediatR;
using Respawn;
using stackblob.Application.Interfaces.Services;
using stackblob.Domain.ValueObjects;
using MongoDB.Bson;

namespace Application.IntegrationTests;

public class SetupFixture : IDisposable
{
    private readonly IConfigurationRoot _configuration;
    public IServiceScopeFactory _scopeFactory;
    private ObjectId _currentUserId = ObjectId.Empty;

    public ObjectId CurrentUserId
    {
        set { _currentUserId = value; } 
        get { return _currentUserId; } 
    }

    private bool _currentUserIsVerified = true;

    public bool CurrentUserIsVerified
    {
        set { _currentUserIsVerified = value; }
        get { return _currentUserIsVerified; }
    }

    private IpAddress? _currentIpAddress = IpAddress.From("5.75.168.140");

    public IpAddress? CurrentUserIpAddress
    {
        set { _currentIpAddress = value; }
        get { return _currentIpAddress; }
    }

    public SetupFixture()
    {
        //var builder = new ConfigurationBuilder().a
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", true, true)
            .AddEnvironmentVariables();


        _configuration = builder.Build();

        var startup = new Startup(_configuration);

        var services = new ServiceCollection();




        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == Assembly.GetAssembly(typeof(Startup))!.FullName));

        startup.ConfigureServices(services);


        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserService));

        if (currentUserServiceDescriptor != null)
        {
            services.Remove(currentUserServiceDescriptor);
        }

        // Register testing version
        services.AddTransient(provider =>
        {
            var mock = new Mock<ICurrentUserService>();
            mock.SetupGet(c => c.UserId).Returns(() => CurrentUserId);
            mock.SetupGet(c => c.IpAddress).Returns(() => CurrentUserIpAddress);
            mock.SetupGet(c => c.IsVerified).Returns(() => CurrentUserIsVerified);
            return mock.Object;
        });


        _scopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();


        CreateDB();
    }

    private void CreateDB()
    {
        //only asp net core creates scopes automatically (e.g for every webrequest)
        //-> we have to create scopes our own
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StackblobDbContext>();

        context.Database.EnsureCreated();
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }


    public void Dispose()
    {
        
    }
}

[CollectionDefinition("global")]
public class SetupFixtureCollectionDefinition : ICollectionFixture<SetupFixture>
{

}
