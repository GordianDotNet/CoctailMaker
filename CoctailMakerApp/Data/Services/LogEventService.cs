using Blazored.Toast.Services;
using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{

    public class LogEventService : DatabaseServiceBase
    {
        public LogEventService(ILogger<LogEventService> logger) : base(logger)
        {
        }

        public Task<List<LogEvent>> LoadAll()
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    return dbContext.LogEvents.ToList();
                }
            });
        }

        public Task Save(string message, LogEventType logEventType = LogEventType.Debug, [CallerMemberName] string funcName = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            return Save(new LogEvent
            {
                Type = logEventType,
                Message = message,
                Func = funcName,
                File = file,
                Line = line
            });
        }

        private Task Save(LogEvent logEvent)
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
