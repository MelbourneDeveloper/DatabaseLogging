using DatabaseLogging.Db;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;


namespace DatabaseLogging
{
    public class DatabaseLoggerProvider : ILoggerProvider, IDisposable, ISupportExternalScope
    {
        #region Private Fields

        private readonly ConcurrentDictionary<string, DatabaseLogger> _loggers = new ConcurrentDictionary<string, DatabaseLogger>();
#pragma warning disable CA2213 // Disposable fields should be disposed
        private readonly IMemoryCache memoryCache;
#pragma warning restore CA2213 // Disposable fields should be disposed
        private IDatabaseLoggerOptions options;
        private IDisposable? optionsReloadToken;
        IExternalScopeProvider scopeProvider;

        #endregion Private Fields

        #region Public Constructors

        public DatabaseLoggerProvider(IOptionsMonitor<DatabaseLoggerOptions> options, IExternalScopeProvider externalScopeProvider)
        {
            this.scopeProvider = externalScopeProvider ?? new LoggerExternalScopeProvider();
            this.options = options?.CurrentValue ?? throw new InvalidOperationException();
            memoryCache = new MemoryCache(new MemoryCacheOptions());

            // Filter would be applied on LoggerFactory level
            optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            ReloadLoggerOptions(options.CurrentValue);
        }

        #endregion Public Constructors

        #region Public Methods

        public ILogger CreateLogger(string categoryName)
        {
            var databaseLoggerProvider = _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
            return databaseLoggerProvider;
        }

        public void Dispose()
        {
            optionsReloadToken?.Dispose();
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        #endregion Public Methods

        #region Private Methods

        private DatabaseLogger CreateLoggerImplementation(string name)
        {
            return new DatabaseLogger(name, options, memoryCache, scopeProvider);
        }

        private void ReloadLoggerOptions(DatabaseLoggerOptions options)
        {
            this.options = options;

            foreach (var logger in _loggers.Values)
            {
                logger.settings = options;
            }
        }

        #endregion Private Methods
    }
}
