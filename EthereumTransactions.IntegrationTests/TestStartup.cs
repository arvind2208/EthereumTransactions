using EthereumTransactions.Middlewares;
using EthereumTransactions.Options;
using EthereumTransactions.Services;
using EthereumTransactions.Services.ThirdPartyServices.Infura;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EthereumTransactions.IntegrationTests
{
	public class TestStartup
	{
		public TestStartup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddOptions()
				.Configure<EthereumOptions>(Configuration.GetSection("EthereumOptions"));

			services.AddMvcCore()
			   .AddDataAnnotations()
			   .AddApiExplorer();

			services.AddControllers();

			services.AddScoped<ITransactionSearchService, TransactionSearchService>();

			services.AddHttpContextAccessor().AddAutoMapper(typeof(Startup));

			services.AddHttpClient<IEthereumService, FakeEthereumService>()
				.SetHandlerLifetime(TimeSpan.FromMinutes(5));
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseRouting();

			app.UseMiddleware<ExceptionHandlingMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
