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
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

namespace stackblob.Infrastructure;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<HttpClient>();


        /*
        *  Configure EF
        */

        var connectionString = "";
#if DEBUG
        connectionString = configuration.GetConnectionString("DevConnection");
#else
        connectionString = configuration.GetConnectionString("DefaultConnection");
#endif

        GlobalUtil.ConnectionString = connectionString;

        if (configuration.GetValue<bool>("IsMongoDb"))
        {
            GlobalUtil.MongoDbName = configuration.GetConnectionString("DBName") ?? "";
           
            var mongoDb = new MongoClient(connectionString).GetDatabase(GlobalUtil.MongoDbName);



            services.AddDbContext<StackblobDbContext>(options =>
               options.UseMongoDB(
                   mongoDb.Client,
                   mongoDb.DatabaseNamespace.DatabaseName
                )
               );
        }
        else
        {
            services.AddDbContext<StackblobDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(StackblobDbContext).Assembly.FullName))
                );
        }

        services.AddScoped<IStackblobDbContext>(provider => provider.GetRequiredService<StackblobDbContext>());

        services.Configure<AppConfig>(configuration);

        if(configuration.GetValue<bool>("IsMongoDb"))
        {
            GlobalUtil.IsMongoDb = true;
        }



        /*
        * Logging 
        */
        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"));
            builder.AddConsole();
        });

        services.AddTransient(typeof(ILoggerService<>), typeof(LoggerService<>));


        var connectionStrs = configuration.GetSection("ConnectionStrings");
        services.Configure<ConnectionStringOptions>(connectionStrs);


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


        /*
        * Mail service 
        */
        var mailSection = configuration.GetSection("MailSettings");
        services.Configure<MailSettings>(mailSection);

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