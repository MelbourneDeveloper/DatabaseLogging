using Microsoft.Extensions.Logging;
using System;
using System.Collections.Immutable;

namespace DatabaseLogging.Model
{
    public record LogMessageRecord
    (
         LogLevel LogLevel,
         int EventId,
         string? EventName,
         string? Exception,
         string Message,
         DateTimeOffset LogDateTime,
         ImmutableList<LogPropertyRecord> LogProperties
    );
}
