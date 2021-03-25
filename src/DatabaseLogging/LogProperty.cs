using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLogging
{
    public class LogProperty
    {
        public LogProperty(Guid key, Guid logPropertyKeyKey, string value)
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
