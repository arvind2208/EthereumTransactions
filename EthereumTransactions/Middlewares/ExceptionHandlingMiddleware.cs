using EthereumTransactions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace EthereumTransactions.Middlewares
{
	public class ExceptionHandlingMiddleware
	{
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;
		private readonly RequestDelegate _next;

		public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				await WriteResponseAsync(context, e);
			}
		}


		private async Task WriteResponseAsync(HttpContext context, Exception e)
		{
			context.Response.ContentType = "application/json";
			_logger.LogError(e, "Unexpected error");
				var result = new ApiResult<object>
				{
					Message = "Unexpected error: " + e.Message
				};
			string responseContent = JsonConvert.SerializeObject(result);
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;

			await context.Response.WriteAsync(responseContent);
		}

	}
}
