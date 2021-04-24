using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EthereumTransactions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace EthereumTransactions.Handlers
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LoggingMessageHandler> _logger;

        public LoggingMessageHandler(ILogger<LoggingMessageHandler> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;

            // not using DI as this may not be available in  GenericHost
            _httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var correlationId = GetOrCreateCorrelationId(request);

            var stopwatch = Stopwatch.StartNew();

            LogContext.Push(new CorrelationIdEnricher(() => correlationId));
            LogRequestStart(request);

            HttpResponseMessage response = null;
            try
            {
                response = await base.SendAsync(request, cancellationToken);
                LogRequestEnd(response, stopwatch.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {
                LogRequestEnd(response, stopwatch.ElapsedMilliseconds, ex);
                throw;
            }
        }

        private void LogRequestStart(HttpRequestMessage request)
        {
            LogContext.PushProperty("ApiRequestMethod", request.Method);
            LogContext.PushProperty("ApiRequestPath", request.RequestUri);
            _logger.LogInformation("ApiRequestBegin");
        }

        private void LogRequestEnd(HttpResponseMessage response, double elapsedMs,
            Exception ex = null)
        {
            LogContext.PushProperty("ApiStatusCode", response?.StatusCode);
            LogContext.PushProperty("ApiRequestDuration", elapsedMs);
            LogContext.PushProperty("ApiRequestSuccessful", response?.IsSuccessStatusCode ?? false);
            _logger.LogInformation(ex, "ApiRequestEnd");
        }

        private string GetOrCreateCorrelationId(HttpRequestMessage request)
        {
            string correlationId;

            if (!request.Headers.TryGetValues("CorrelationId", out var values))
            {
                correlationId = _httpContextAccessor != null
                    ? _httpContextAccessor.HttpContext.GetOrCreateCorrelationId()
                    : Guid.NewGuid().ToString();


                request.Headers.Add("CorrelationId", correlationId);
            }
            else
            {
                correlationId = values.FirstOrDefault();
            }

            return correlationId;
        }
    }
}
