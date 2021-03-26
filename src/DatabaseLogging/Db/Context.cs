using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseLogging.Db
{
    public class Context : DbContext
    {
        Action<DbContextOptionsBuilder> configureAction;

        public Context(Action<DbContextOptionsBuilder> configureAction)
        {
            this.configureAction = configureAction;
            _ = Database.EnsureCreated();
        }

        public LogMessages LogMessages { get; set; } = new LogMessages();
        public LogPropertyKeys LogPropertyKeys { get; set; } = new LogPropertyKeys();
        public LogPropertyValues LogPropertyValues { get; set; } = new LogPropertyValues();
        public LogEvents LogEvents { get; set; } = new LogEvents();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configureAction(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
