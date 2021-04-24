using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using EthereumTransactions.Extensions;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace EthereumTransactions.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
               .UseKestrel()
               .UseIISIntegration()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration(builder =>
               {
                   var integrationConfig = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build();

                   builder.AddConfiguration(integrationConfig);
               })
               .UseStartup<TestStartup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                }
            });
        }

        //protected override IHostBuilder CreateHostBuilder()
        //{
        //    return Host.CreateDefaultBuilder()
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .ConfigureAppConfiguration((hostingContext, config) =>
        //        {
        //            var env = hostingContext.HostingEnvironment.EnvironmentName;

        //            config
        //                .AddJsonFile("appsettings.json", true, true)
        //                .AddJsonFile($"appsettings.{env}.json", true, true)
        //                //.AddSystemsManager($"/Ethereum/{env}", optional: true) // replace any secrets from a vault
        //                .AddEnvironmentVariables();
        //        })
        //        .UseSerilog((context, loggerConfiguration) => { loggerConfiguration.Configure(context); })
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<TestStartup>();
        //        });
        //}

        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.ConfigureServices((services) =>
        //    {
        //        services.BuildServiceProvider();
        //    });
        //}
    }
}
