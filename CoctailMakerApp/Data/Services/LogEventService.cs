using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{

    public class LogEventService : DatabaseServiceBase
    {
        public Task<List<LogEvent>> GetLogEvents()
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    return dbContext.LogEvents.ToList();
                }
            });
        }

        public Task AddLogEvent(string message, LogEventType logEventType = LogEventType.Debug, [CallerMemberName] string funcName = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            return AddLogEvent(new LogEvent
            {
                Type = logEventType,
                Message = message,
                Func = funcName,
                File = file,
                Line = line
            });
        }

        private Task AddLogEvent(LogEvent logEvent)
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    dbContext.LogEvents.Add(logEvent);
                    dbContext.SaveChanges();
                }
            });
        }
    }
}
