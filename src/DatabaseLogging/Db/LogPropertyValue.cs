using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLogging
{
    public class LogPropertyValue
    {
        public LogPropertyValue(Guid key, Guid logPropertyKeyKey, string value)
        {
            Key = key;
            LogPropertyKeyKey = logPropertyKeyKey;
            Value = value;
        }

        [Key]
        public Guid Key { get; }
        public Guid LogPropertyKeyKey { get; }
        public string Value { get; }
    }
}
