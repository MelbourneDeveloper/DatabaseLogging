using DatabaseLogging.Db;
using System;
using System.Threading;

namespace DatabaseLogging
{
    public interface IDatabaseLoggerSettings
    {
        bool IncludeScopes { get; } 
        ThreadPriority ThreadPriority { get; }
        Func<Context> GetDatabaseContext { get; }
        TimeSpan ProcessingDelay { get; }
    }
}