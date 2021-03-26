using DatabaseLogging.Db;
using System;
using System.Threading;

namespace DatabaseLogging
{
    public interface IDatabaseLoggerOptions
    {
        bool IncludeScopes { get; } 
        ThreadPriority ThreadPriority { get; }
        Func<Context> GetDatabaseContext { get; }
        TimeSpan ProcessingDelay { get; }
    }
}