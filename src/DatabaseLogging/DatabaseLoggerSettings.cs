using DatabaseLogging.Db;
using System;
using System.Threading;

namespace DatabaseLogging
{
    public class DatabaseLoggerSettings : IDatabaseLoggerSettings
    {
        public DatabaseLoggerSettings(Func<Context> getDatabaseContext)
        {
            GetDatabaseContext = getDatabaseContext;
        }

        public ThreadPriority ThreadPriority { get; set; } = ThreadPriority.BelowNormal;

        public Func<Context> GetDatabaseContext { get; }

        public bool IncludeScopes { get; set; } = true;

        public TimeSpan ProcessingDelay => new TimeSpan(0, 0, 0, 0, 10);
    }
}