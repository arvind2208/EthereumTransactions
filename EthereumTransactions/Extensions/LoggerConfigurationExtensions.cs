using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace EthereumTransactions.Extensions
{
	public static class LoggerConfigurationExtensions
	{
		public static LoggerConfiguration Configure(this LoggerConfiguration configuration, HostBuilderContext context = null)
		{
			configuration
				.MinimumLevel.Verbose()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.MinimumLevel.Override("System", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails();

			if (context != null)
				configuration.ReadFrom.Configuration(context.Configuration);

			configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code);

			return configuration;
		}
	}
}
