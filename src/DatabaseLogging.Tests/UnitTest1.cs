using DatabaseLogging.Db;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
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

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<TestClass>();

            //using var scope = logger.BeginScope("Scope: {someArg}", 456);
            logger.LogInformation("Test {Hi}", 123);

            await Task.Delay(1000).ConfigureAwait(false);
        }
    }

    public class TestClass
    {

    }
}
