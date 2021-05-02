using EthereumTransactions.Handlers;
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

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddOptions()
				.Configure<EthereumOptions>(Configuration.GetSection("EthereumOptions"));

			services
				.AddTransient<LoggingMessageHandler>()
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

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseCors(
				options => options.AllowAnyOrigin()
								  .AllowAnyHeader()
								  .AllowAnyMethod()
			);

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
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
