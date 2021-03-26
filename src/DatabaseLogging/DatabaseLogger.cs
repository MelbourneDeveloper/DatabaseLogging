using DatabaseLogging.Db;
using DatabaseLogging.Model;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace DatabaseLogging
{
    public class DatabaseLogger : ILogger, IDisposable
    {
        #region Internal Fields

        internal IDatabaseLoggerOptions settings;

        #endregion Internal Fields

        #region Private Fields

        private readonly Queue<LogMessageRecord> pendingLogs = new();
        Context context;
        private bool disposed;
        private IMemoryCache memoryCache;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public string Name { get; }

        #endregion Public Properties

        #region Internal Properties

        internal IExternalScopeProvider ScopeProvider { get; set; }

        #endregion Internal Properties

        #region Public Methods

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;

        public void Dispose()
        {
            disposed = true;
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

        #endregion Public Methods

        #region Private Methods

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

                    //TODO: Add scope
                    //var scopeProvider = ScopeProvider;
                    //if (scopeProvider != null)
                    //{
                    //    var stringBuilder = new StringBuilder();
                    //    var initialLength = stringBuilder.Length;

                    //    scopeProvider.ForEachScope((scope, state) =>
                    //    {
                    //        var (builder, length) = state;
                    //        var first = length == builder.Length;
                    //        builder.Append(first ? "=> " : " => ").Append(scope);
                    //    }, (stringBuilder, initialLength));                       
                    //}

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
                    Thread.Sleep(settings.ProcessingDelay);
                }
            }
        }

        #endregion Private Methods
    }
}
