using System.Threading;

namespace DatabaseLogging
{
    internal class DatabaseLoggerSettings : IDatabaseLoggerSettings
    {
        public bool? IncludeScopes { get; set; }

        public ThreadPriority ThreadPriority { get; set; }
    }
}