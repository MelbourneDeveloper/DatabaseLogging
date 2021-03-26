using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLogging.Db
{
    public class LogMessage
    {
        public LogMessage() : this(Guid.NewGuid(), LogLevel.Trace, default, null, "", DateTimeOffset.UtcNow, new List<LogPropertyValue>())
        {
            Message = "";
        }

        public LogMessage
            (
         Guid key,
         LogLevel logLevel,
         Guid logEventKey,
         string? exception,
         string message,
         DateTimeOffset logDateTime,
#pragma warning disable CA1002 // Do not expose generic lists
         List<LogPropertyValue> logProperties
#pragma warning restore CA1002 // Do not expose generic lists
            )
        {
            Key = key;
            LogLevel = logLevel;
            LogEventKey = logEventKey;
            Exception = exception;
            Message = message;
            LogDateTime = logDateTime;
            LogProperties = logProperties;
        }

        [Key]
        public Guid Key { get; set; }
        public LogLevel LogLevel { get; set; }
        [ForeignKey("LogEvent")]
        public Guid LogEventKey { get; set; }
        public LogEvent? LogEvent { get; set; }
        public string? Exception { get; set; }
        public string Message { get; set; }
        public DateTimeOffset LogDateTime { get; set; }
#pragma warning disable CA1002 // Do not expose generic lists
        public List<LogPropertyValue> LogProperties { get; }
#pragma warning restore CA1002 // Do not expose generic lists
    }

}
