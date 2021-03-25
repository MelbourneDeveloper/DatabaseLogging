using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DatabaseLogging
{
    public class DatabaseLogger : ILogger
    {
        private readonly Queue<LogMessage> pendingLogs = new();

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) return;

            var message = formatter(state, exception);

            var logProperties = ImmutableList<LogProperty>.Empty;

            if (state is IReadOnlyList<KeyValuePair<string, object>> kvps)
            {
                logProperties = ImmutableList.Create(kvps.Select(kvp => new LogProperty(kvp.Key, kvp.Value.ToString())).ToArray());
            }

            pendingLogs.Enqueue(new LogMessage(Guid.NewGuid(), logLevel, eventId.Id, exception?.ToString(), message, DateTimeOffset.UtcNow, logProperties));
        }
    }

}
