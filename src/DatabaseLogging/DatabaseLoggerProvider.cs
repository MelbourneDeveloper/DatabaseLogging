using DatabaseLogging.Db;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace DatabaseLogging
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();
        private IDatabaseLoggerSettings _settings;
        private bool _includeScopes;
        Func<Context> getContext;
        IMemoryCache memoryCache;

        public DatabaseLoggerProvider(
            bool includeScopes,
            Func<Context> getContext,
            IMemoryCache memoryCache,
            IDatabaseLoggerSettings settings)
        {
            _includeScopes = includeScopes;
            this.getContext = getContext;
            this.memoryCache = memoryCache;
            _settings = settings;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var databaseLoggerProvider = _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
            return databaseLoggerProvider;
        }

        private DatabaseLogger CreateLoggerImplementation(string name)
        {
            var includeScopes = _settings?.IncludeScopes ?? _includeScopes;

            return new DatabaseLogger(name, getContext(), _settings?.ThreadPriority ?? System.Threading.ThreadPriority.BelowNormal, memoryCache);
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
        }
    }
}
