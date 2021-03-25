namespace DatabaseLogging
{
    public record LogMessage
    (
        LogLevel LogLevel,
        int EventId,
        string? Exception,
        string Message,
        DateTimeOffset LogDateTime,
        ImmutableList<LogProperty> LogProperties
    );

}
