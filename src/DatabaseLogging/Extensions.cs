using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using System;

namespace DatabaseLogging
{
    public static class DatabaseLoggingExtensions
    {
        public static ILoggingBuilder AddDatabase(this ILoggingBuilder builder, Action<DatabaseLoggerOptions> configure)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            builder.AddDatabase();
            builder.Services.Configure(configure);

            return builder;
        }

        public static ILoggingBuilder AddDatabase(this ILoggingBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, DatabaseLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<DatabaseLoggerOptions, DatabaseLoggerProvider>(builder.Services);

            //Is this good????
            builder.Services.AddSingleton<IExternalScopeProvider, LoggerExternalScopeProvider>();
#pragma warning disable CA2000 // Dispose objects before losing scope
            builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions())); ;
#pragma warning restore CA2000 // Dispose objects before losing scope

            return builder;
        }
    }
}
