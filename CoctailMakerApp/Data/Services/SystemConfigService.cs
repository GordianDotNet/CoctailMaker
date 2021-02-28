using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{
    public class SystemConfigService : DatabaseServiceBase
    {
        public Task<SystemConfig> Load()
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    return dbContext.SystemJsonConfigs.OrderByDescending(x => x.Id).FirstOrDefault()?.Content ?? new SystemConfig();
                }
            });
        }

        public Task Save(SystemConfig systemConfig)
        {
            systemConfig?.UpdateIngredientIds();

            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    dbContext.SystemJsonConfigs.Add(new SystemJsonConfig { Content = systemConfig });
                    dbContext.SaveChanges();
                }
            });
        }
    }
}
