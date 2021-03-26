using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseLogging.Db
{
    public class Context : DbContext
    {
        Action<DbContextOptionsBuilder> configureAction;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Context(Action<DbContextOptionsBuilder> configureAction)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.configureAction = configureAction;
            _ = Database.EnsureCreated();
        }

        public DbSet<LogMessage> LogMessages { get; set; }
        public DbSet<LogPropertyKey> LogPropertyKeys { get; set; }
        public DbSet<LogPropertyValue> LogPropertyValues { get; set; }
        public DbSet<LogEvent> LogEvents { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configureAction(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
