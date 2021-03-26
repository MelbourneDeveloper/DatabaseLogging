using DatabaseLogging.Db;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;

#pragma warning disable CA1063 // Implement IDisposable Correctly

namespace DatabaseLogging
{
    public class DatabaseLoggerProvider : ILoggerProvider, IDisposable
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();
        private IDatabaseLoggerOptions options;
        private IDisposable? optionsReloadToken;
#pragma warning disable CA2213 // Disposable fields should be disposed
        private readonly IMemoryCache memoryCache;
#pragma warning restore CA2213 // Disposable fields should be disposed

        public DatabaseLoggerProvider(IOptionsMonitor<DatabaseLoggerOptions> options)
        {
            this.options = options?.CurrentValue ?? throw new InvalidOperationException();
            memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Filter would be applied on LoggerFactory level
            optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            ReloadLoggerOptions(options.CurrentValue);
        }

        private void ReloadLoggerOptions(DatabaseLoggerOptions options)
        {
            this.options = options;

            foreach (var logger in _loggers.Values)
            {
                logger.settings = options;
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            var databaseLoggerProvider = _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
            return databaseLoggerProvider;
        }

        private DatabaseLogger CreateLoggerImplementation(string name)
        {
            return new DatabaseLogger(name, options, memoryCache);
        }

#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
            optionsReloadToken?.Dispose();
        }
    }
}
