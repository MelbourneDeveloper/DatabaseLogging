# DatabaseLogging

Send ASP.NET Core logs to any Entity Framework database (`ILogger` / `ILoggerFactory`)

PS: Your app doesn't need to ASP.NET Core. You just have to use `Microsoft.Extensions.Logging`

## Quick Start

- Add Nuget `DatabaseLogging` (currently alpha so be sure to include pre-release)
- Add the Entity Framework NuGet for the database you want to use. (E.g. `Microsoft.EntityFrameworkCore.Sqlite` or 'Microsoft.EntityFrameworkCore.SqlServer')
- Call AddDatabase and initialize the `DbContext` with code for the database platform

### SQL Server

```cs
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging((builder) =>
{
    builder.AddDatabase((options) =>
    {
        options.GetDatabaseContext = () =>
        {
            var context = new Context((builder) =>
            {
                builder.UseSqlServer("Server=(local);Database=Logging;Trusted_Connection=True;MultipleActiveResultSets=true");
            });

            return context;
        };
    });
});
```

### SQLite

```cs
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging((builder) =>
{
    builder.AddDatabase((options) =>
    {
        options.GetDatabaseContext = () =>
        {
            var context = new Context((builder) =>
            {
                var connection = new SqliteConnection("Data Source=Log.db");
                connection.Open();

                var command = connection.CreateCommand();

                //Create the database if it doesn't already exist
                command.CommandText = "PRAGMA foreign_keys = ON;";
                _ = command.ExecuteNonQuery();
                _ = builder.UseSqlite(connection);
            });

            return context;
        };
    });
});
```

### Create the Logger 

```cs
var serviceProvider = serviceCollection.BuildServiceProvider();
var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger<TestClass>();
logger.LogInformation("Test {Hi}", 123);
```
