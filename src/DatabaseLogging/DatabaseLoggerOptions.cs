using DatabaseLogging.Db;
using System;
using System.Threading;

namespace DatabaseLogging
{
    public class DatabaseLoggerOptions : IDatabaseLoggerOptions
    {
  
        public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.BelowNormal;

        public Func<Context> GetDatabaseContext { get; set; } = () => throw new InvalidOperationException($"You must initialize {nameof(GetDatabaseContext)} in the options for DatabaseLogging");

        public bool IncludeScopes { get; set; } = true;

        public TimeSpan ProcessingDelay => new TimeSpan(0, 0, 0, 0, 10);
    }
}