using DatabaseLogging.Db;
using DatabaseLogging.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace DatabaseLogging
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class DatabaseLogger : ILogger, IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        private readonly Queue<LogMessageRecord> pendingLogs = new();
        private bool disposed;
        private IMemoryCache memoryCache;
        internal IDatabaseLoggerOptions settings;
        Context context;
        internal IExternalScopeProvider ScopeProvider { get; set; }

        public string Name { get; }

        public DatabaseLogger(
            string name,
            IDatabaseLoggerOptions settings,
            IMemoryCache memoryCache,
            IExternalScopeProvider externalScopeProvider)
        {

            ScopeProvider = externalScopeProvider;
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            Name = name;
            context = settings.GetDatabaseContext();
            this.memoryCache = memoryCache;
            new Thread(ProcessLogs) { Priority = settings.ThreadPriority }.Start();
        }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;

        private void ProcessLogs(object obj)
        {
            while (!disposed || pendingLogs.Count > 0)
            {
                if (pendingLogs.Count > 0)
                {
                    var logMessageRecord = pendingLogs.Dequeue();

                    var logEvent = memoryCache.GetOrCreate($"{nameof(LogEvent)}-{logMessageRecord.EventId}", (entry) =>
                     {
                         var logEvent = context.LogEvents.Where(lpk => lpk.Id == logMessageRecord.EventId).FirstOrDefault();

                         if (logEvent != null) return logEvent;

                         logEvent = new LogEvent { Key = Guid.NewGuid(), Id = logMessageRecord.EventId, Name = logMessageRecord.EventName };

                         context.LogEvents.Add(logEvent);

                         return logEvent;
                     });

                    var logMessage = new LogMessage(
                        Guid.NewGuid(),
                        Name,
                        logMessageRecord.LogLevel,
                        logEvent.Key,
                        logMessageRecord.Exception,
                        logMessageRecord.Message,
                        logMessageRecord.LogDateTime,
                        logMessageRecord.LogProperties.Select(lp => new LogPropertyValue
                        (
                            Guid.NewGuid(),
                            memoryCache.GetOrCreate($"{nameof(LogPropertyKey)}-{lp.Key}", (entry) =>
                            {
                                var logPropertyKey = context.LogPropertyKeys.FirstOrDefault(lpk => lpk.KeyName == lp.Key);

                                if (logPropertyKey != null) return logPropertyKey;

                                logPropertyKey = new LogPropertyKey(Guid.NewGuid(), lp.Key);

                                context.LogPropertyKeys.Add(logPropertyKey);

                                return logPropertyKey;
                            }).Key, false, lp.Value
                            )).ToList()
                        );

                    context.LogMessages.Add(logMessage);

                    context.SaveChanges();
                }
                else
                {
                    Thread.Sleep(10);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null) return;

            var message = formatter(state, exception);

            var logProperties = ImmutableList<LogPropertyRecord>.Empty;

            if (state is IReadOnlyList<KeyValuePair<string, object>> kvps)
            {
                logProperties = ImmutableList.Create(kvps.Select(kvp => new LogPropertyRecord(kvp.Key, kvp.Value.ToString())).ToArray());
            }

            pendingLogs.Enqueue(new LogMessageRecord(
                logLevel,
                eventId.Id,
                //Why does NRT not pick this up as possibly null? Another NRT bug?
                eventId.Name ?? "",
                exception?.ToString(),
                message,
                DateTimeOffset.UtcNow,
                logProperties));
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            disposed = true;
        }
    }
}
