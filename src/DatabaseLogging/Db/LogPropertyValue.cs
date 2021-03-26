using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseLogging.Db
{
    public class LogPropertyValue
    {
        public LogPropertyValue() : this(Guid.NewGuid())
        {

        }

        public LogPropertyValue(Guid key, Guid logPropertyKeyKey = default, bool isScopedProperty = false, string value = "")
        {
            Key = key;
            LogPropertyKeyKey = logPropertyKeyKey;
            Value = value;
            IsScopedProperty = isScopedProperty;
        }

        [Key]
        public Guid Key { get; set; }
        [ForeignKey("LogPropertyKey")]
        public Guid LogPropertyKeyKey { get; set; }
        public LogPropertyKey? LogPropertyKey { get; set; }
        public string Value { get; set; }
        public bool IsScopedProperty { get; set; }
    }
}
