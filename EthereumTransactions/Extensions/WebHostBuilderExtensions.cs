using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;


namespace EthereumTransactions.Extensions
{
	public static class WebHostBuilderExtensions
	{
		public static IHostBuilder UseDefaultConfiguration(this IHostBuilder builder)
		{
			return builder
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureAppConfiguration((hostingContext, config) =>
				{
					var env = hostingContext.HostingEnvironment.EnvironmentName;

					config
						.AddJsonFile("appsettings.json", true, true)
						.AddJsonFile($"appsettings.{env}.json", true, true)
						//.AddSystemsManager($"/Ethereum/{env}", optional: true) // replace any secrets from a vault
						.AddEnvironmentVariables();
				})
				.UseSerilog((context, loggerConfiguration) => { loggerConfiguration.Configure(context); });
		}
	}
}
