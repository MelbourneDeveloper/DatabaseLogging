using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLogging.Db
{
    public class LogPropertyKey
    {
        public LogPropertyKey() : this(Guid.NewGuid())
        {

        }

        public LogPropertyKey(Guid key, string keyName = "")
        {
            Key = key;
            KeyName = keyName;
        }

        [Key]
        public Guid Key { get; set; }
        public string KeyName { get; set; }
    }
}
