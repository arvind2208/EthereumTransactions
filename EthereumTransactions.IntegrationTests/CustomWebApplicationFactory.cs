using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

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
    }
}
