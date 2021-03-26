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
        }

        public LogMessages LogMessages { get; } = new LogMessages();
        public LogPropertyKeys LogPropertyKeys { get; } = new LogPropertyKeys();
        public LogPropertyValues LogPropertyValues { get; } = new LogPropertyValues();
        public LogEvents LogEvents { get; } = new LogEvents();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            configureAction(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
