using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace EthereumTransactions.Extensions
{
    public static class HttpContextExtensions
    {
        internal static string GetOrCreateCorrelationId(this HttpContext context)
        {
            if (context.Items.TryGetValue("CorrelationId", out var value) && value is string s)
            {
                return s;
            }

            var header = string.Empty;

            if (context.Request.Headers.TryGetValue("CorrelationId", out var values))
            {
                header = values.FirstOrDefault();
            }

            var correlationId = string.IsNullOrEmpty(header)
                ? Guid.NewGuid().ToString()
                : header;

            context.Items["CorrelationId"] = correlationId;

            if (!context.Response.Headers.ContainsKey("CorrelationId"))
            {
                context.Response.Headers.Add("CorrelationId", correlationId);
            }

            return correlationId;
        }
    }
}
