using DatabaseLogging.Db;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace DatabaseLogging
{
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class DatabaseLoggerProvider : ILoggerProvider
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();
        private IDatabaseLoggerOptions settings;
        IMemoryCache memoryCache;

        public DatabaseLoggerProvider(
            IMemoryCache memoryCache,
            IDatabaseLoggerOptions settings)
        {
            this.memoryCache = memoryCache;
            this.settings = settings;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var databaseLoggerProvider = _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
            return databaseLoggerProvider;
        }

        private DatabaseLogger CreateLoggerImplementation(string name)
        {
            return new DatabaseLogger(name, settings, memoryCache);
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
