using System;
using Serilog.Core;
using Serilog.Events;

namespace EthereumTransactions.Handlers
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        private readonly Func<string> _correlationIdGenerator;

        public CorrelationIdEnricher(Func<string> correlationIdGenerator)
        {
            _correlationIdGenerator = correlationIdGenerator;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationId = _correlationIdGenerator.Invoke();

            var correlationIdProperty = propertyFactory.CreateProperty("CorrelationId", correlationId);

            logEvent.AddOrUpdateProperty(correlationIdProperty);
        }
    }
}
