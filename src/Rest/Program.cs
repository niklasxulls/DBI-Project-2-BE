using stackblob.Application.Interfaces;
using stackblob.Application.Interfaces.Services;
using stackblob.Infrastructure.Persistence;
using stackblob.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using stackblob.Domain.Settings;

namespace Rest
{
    public class Program
    {
        //abc
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<StackblobMongoRELDbContext>();

                    if(!GlobalUtil.IsMongoDb)
                    {
                        context.Database.EnsureCreated();
                    }

                    await StackblobDbContextSeed.SeedSampleData(context);
                }
                catch(Exception e)
                {
                    var x = 2;
                }
                finally
                {
                    await host.RunAsync();
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //just for development |>
                    webBuilder.CaptureStartupErrors(true).UseSetting("detailedErrors", "true");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
