using Microsoft.Extensions.Logging;
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLogging.Db
{
    public class LogMessage
    {
        public LogMessage
            (
         Guid key,
         LogLevel logLevel,
         Guid eventIdKey,
         string? exception,
         string message,
         DateTimeOffset logDateTime,
         ImmutableList<LogPropertyValue> logProperties
            )
        {
            Key = key;
            LogLevel = logLevel;
            EventIdKey = eventIdKey;
            Exception = exception;
            Message = message;
            LogDateTime = logDateTime;
            LogProperties = logProperties;
        }

        [Key]
        public Guid Key { get; }
        public LogLevel LogLevel { get; }
        public Guid EventIdKey { get; }
        public string? Exception { get; }
        public string Message { get; }
        public DateTimeOffset LogDateTime { get; }
        public ImmutableList<LogPropertyValue> LogProperties { get; }
    }

}
