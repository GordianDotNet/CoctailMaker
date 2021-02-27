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
                    return dbContext.SystemJsonConfigs.OrderByDescending(x => x.Id).FirstOrDefault()?.GetConfig() ?? new SystemConfig();
                }
            });
        }

        public Task Save(SystemConfig systemConfig)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    dbContext.SystemJsonConfigs.Add(new SystemJsonConfig { Config = systemConfig });
                    dbContext.SaveChanges();
                }
            });
        }
    }
}
