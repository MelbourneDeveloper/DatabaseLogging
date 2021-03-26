using System.Threading;

namespace DatabaseLogging
{
    public interface IDatabaseLoggerSettings
    {
        bool? IncludeScopes { get; }
        ThreadPriority ThreadPriority { get; }
    }
}