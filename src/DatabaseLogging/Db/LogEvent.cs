using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseLogging.Db
{
    public class LogEvent
    {
        [Key]
        public Guid Key { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
