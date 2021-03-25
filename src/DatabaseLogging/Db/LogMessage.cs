using Microsoft.Extensions.Logging;
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLogging
{
    public class LogMessage
    {
        public LogMessage
            (
         Guid key,
         LogLevel logLevel,
         int eventId,
         string? exception,
         string message,
         DateTimeOffset logDateTime,
         ImmutableList<LogPropertyValue> logProperties
            )
        {
            Key = key;
            LogLevel = logLevel;
            EventId = eventId;
            Exception = exception;
            Message = message;
            LogDateTime = logDateTime;
            LogProperties = logProperties;
        }

        [Key]
        public Guid Key { get; }
        public LogLevel LogLevel { get; }
        public int EventId { get; }
        public string? Exception { get; }
        public string Message { get; }
        public DateTimeOffset LogDateTime { get; }
        public ImmutableList<LogPropertyValue> LogProperties { get; }
    }

}
