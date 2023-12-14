using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Domain.Settings;
using stackblob.Infrastructure.Persistence;
using stackblob.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Azure.Storage.Blobs;

namespace stackblob.Infrastructure;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<HttpClient>();


        /*
        *  Configure EF
        */
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<StackblobDbContext>(options =>
               options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        }
        else
        {
            services.AddDbContext<StackblobDbContext>(options =>
                options.UseSqlServer(
#if DEBUG
                    configuration.GetConnectionString("DevConnection"),
#else
                    configuration.GetConnectionString("DefaultConnection"),
#endif
                    b => b.MigrationsAssembly(typeof(StackblobDbContext).Assembly.FullName))
                );
        }

        services.AddScoped<IStackblobDbContext>(provider => provider.GetRequiredService<StackblobDbContext>());


        /*
        * Logging 
        */
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));


        /*
        * Auth settings + service 
        */
        var authSection = configuration.GetSection("AuthSettings");
        services.Configure<AuthSettings>(authSection);

        var authSettings = authSection.Get<AuthSettings>();
        var key = Encoding.UTF8.GetBytes(authSettings.SecretKey);

        TokenValidationParameters validationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ValidIssuer = authSettings.Issuer,
            ValidAudience = authSettings.Audience,
            ClockSkew = authSettings.ClockSkew
        };
        services.Configure<TokenValidationParameters>(cfg => cfg = validationParameters);
        services.AddSingleton<TokenValidationParameters>(validationParameters);

        services.AddAuthentication(options =>
        {
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = validationParameters;
        });

        services.AddTransient<IAuthService, AuthService>();


        /*
        * File service 
        */
        var googleCloudBucketSection = configuration.GetSection("GoogleCloudBucketSettings");
        services.Configure<FileSettings>(googleCloudBucketSection);
        var fileSettings = authSection.Get<FileSettings>();
        FileSettings.PublicFileBaseUrl = $"https://storage.googleapis.com/{fileSettings.Bucket}/";

        services.AddSingleton(x =>
        {
            var service = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorageConnection"));
            AttachmentSettings.BaseUrl = service.GetBlobContainerClient("").Uri.AbsoluteUri;
            return service;
        });
        services.AddTransient<IFileService, GoogleCloudFileService>();


        /*
        * Mail service 
        */
        var mailSection = configuration.GetSection("MailSettings");
        services.Configure<MailSettings>(mailSection);
        services.AddTransient<IMailService, MailService>();

        /*
        * Global settings
        */
        var globalSection = configuration.GetSection("GlobalSettings");
        services.Configure<GlobalSettings>(globalSection);


        /*
        *  IP Resolver
        */
        var ipResolverSection = configuration.GetSection("IPAddressResolverSettings");
        services.Configure<IPAddressResolverSettings>(ipResolverSection);

        services.AddTransient<IIPAddressResolverService, IPAddressResolverService>();

    }

    public static void AddInfrastructureApp(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAuthentication();
        app.UseAuthorization();

    }
}