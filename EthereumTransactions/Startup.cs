using EthereumTransactions.Middlewares;
using EthereumTransactions.Options;
using EthereumTransactions.Services;
using EthereumTransactions.Services.ThirdPartyServices.Infura;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using System;

namespace EthereumTransactions
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddOptions()
				.Configure<EthereumOptions>(Configuration.GetSection("EthereumOptions"));

			services
			   .AddMvcCore()
			   .AddDataAnnotations()
			   .AddApiExplorer();

			services.AddControllers();
			services.AddHealthChecks();

			services.AddScoped<ITransactionSearchService, TransactionSearchService>();

			services.AddHttpContextAccessor().AddAutoMapper(typeof(Startup));

			services.AddHttpClient<IEthereumService, EthereumService>()
				.SetHandlerLifetime(TimeSpan.FromMinutes(5));

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "EthereumTransactions API", Version = "v1" });
			});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors(
				options => options.AllowAnyOrigin()
								  .AllowAnyHeader()
								  .AllowAnyMethod()
			);

			app.UseMiddleware<RequestLoggingMiddleware>();
			
			app.UseSwagger();

			app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "EthereumTransactions API V1"); });

			app.UseRouting();

			app.UseHealthChecks("/api/health");

			app.UseSerilogRequestLogging();

			app.UseMiddleware<ExceptionHandlingMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
