using EthereumTransactions.Extensions;
using EthereumTransactions.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EthereumTransactions.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            LogContext.Push(new CorrelationIdEnricher(context.GetOrCreateCorrelationId));

            var timer = Stopwatch.StartNew();

            try
            {
                LogRequestStart(context);
                await _next(context);
                LogRequestEnd(context, context.Response.StatusCode, timer.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                LogRequestEnd(context, (int)HttpStatusCode.InternalServerError, timer.ElapsedMilliseconds, ex);
                throw;
            }
        }

        private void LogRequestStart(HttpContext context)
        {
            LogContext.PushProperty("RequestMethod", context.Request.Method);
            LogContext.PushProperty("RequestPath", GetPath(context));
            _logger.LogInformation("RequestBegin");
        }

        private void LogRequestEnd(
            HttpContext context,
            int statusCode,
            double elapsedMs,
            Exception ex = null)
        {
            LogContext.PushProperty("RequestMethod", context.Request.Method);
            LogContext.PushProperty("RequestPath", GetPath(context));
            LogContext.PushProperty("StatusCode", statusCode);
            LogContext.PushProperty("RequestDuration", elapsedMs);
            _logger.LogInformation(ex, "RequestEnd");
        }

        private static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
        }
    }
}
