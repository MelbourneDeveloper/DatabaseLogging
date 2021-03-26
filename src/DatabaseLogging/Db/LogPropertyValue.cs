using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLogging
{
    public class LogPropertyValue
    {
        public LogPropertyValue(Guid key, Guid logPropertyKeyKey, bool isScopedProperty, string value)
        {
            Key = key;
            LogPropertyKeyKey = logPropertyKeyKey;
            Value = value;
            IsScopedProperty = isScopedProperty;
        }

        [Key]
        public Guid Key { get; }
        public Guid LogPropertyKeyKey { get; }
        public string Value { get; }
        public bool IsScopedProperty { get; }
    }
}
