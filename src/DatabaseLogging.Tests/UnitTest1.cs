using DatabaseLogging.Db;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace DatabaseLogging.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            using var context = new Context((builder) =>
            {
                var connection = new SqliteConnection("Data Source=Log.db");
                connection.Open();

                var command = connection.CreateCommand();

                //Create the database if it doesn't already exist
                command.CommandText = "PRAGMA foreign_keys = ON;";
                _ = command.ExecuteNonQuery();
                _ = builder.UseSqlite(connection);
            });

            using var memoryCache = new MemoryCache(new MemoryCacheOptions());
            using var logger = new DatabaseLogger(context, System.Threading.ThreadPriority.Highest, memoryCache);
            logger.LogInformation("Test {Hi}", 123);

            await Task.Delay(1000).ConfigureAwait(false);
        }
    }
}
