using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
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
#if DEBUG
			// Local debugging with color.
			configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
#endif
			// Cloudwatch logs to allow structured logging and query.
			configuration.WriteTo.Console(new JsonFormatter());

			return configuration;
		}

		public static LoggerConfiguration Configure(this LoggerConfiguration configuration, WebHostBuilderContext context = null)
		{
			configuration
				.MinimumLevel.Verbose()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.MinimumLevel.Override("System", LogEventLevel.Information)
				.Enrich.FromLogContext()
				.Enrich.WithExceptionDetails();

			if (context != null)
				configuration.ReadFrom.Configuration(context.Configuration);
#if DEBUG
			// Local debugging with color.
			configuration.WriteTo.Console(theme: AnsiConsoleTheme.Code);
#endif
			// Cloudwatch logs to allow structured logging and query.
			configuration.WriteTo.Console(new JsonFormatter());

			return configuration;
		}
	}
}
