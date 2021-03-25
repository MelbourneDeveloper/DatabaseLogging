using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLogging
{
    public class Context : DbContext
    {
        public LogMessages LogMessages { get; } = new LogMessages();
        public LogPropertyKeys LogPropertyKeys { get; } = new LogPropertyKeys();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class LogMessages : DbSet<LogMessage>
    {

    }

    public class LogPropertyKeys : DbSet<LogPropertyKey>
    {

    }
}
