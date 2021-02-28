using CoctailMakerApp.Data.Context;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{
    public class DatabaseServiceBase
    {
        public DatabaseServiceBase(ILogger logger)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }

        public Task DeleteDatabase()
        {
            return Task.Run(() =>
            {
                SqliteDbContext.DeleteDatabase();
            });
        }
    }
}
