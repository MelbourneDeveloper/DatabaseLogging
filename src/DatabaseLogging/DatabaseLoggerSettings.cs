using System.Threading;

namespace DatabaseLogging
{
    public class DatabaseLoggerSettings : IDatabaseLoggerSettings
    {
        public bool? IncludeScopes { get; set; }

        public ThreadPriority ThreadPriority { get; set; }
    }
}