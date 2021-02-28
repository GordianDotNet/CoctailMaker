using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Services
{
    public class DatabaseService
    {
        public DatabaseService(ILogger<DatabaseService> logger)
        {
            Logger = logger;
        }

        public ILogger<DatabaseService> Logger { get; }

        public Task DeleteDatabase()
        {
            return Task.Run(() =>
            {
                SqliteDbContext.DeleteDatabase();
            });
        }

        public Task<SystemConfig> LoadSystemConfigOrCreateDefault()
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    return dbContext.SystemConfigs.OrderByDescending(x => x.Id).FirstOrDefault() ?? new SystemConfig();
                }
            });
        }

        public Task SaveLogEvent(string message, LogEventType logEventType = LogEventType.Debug, [CallerMemberName] string funcName = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
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

        public Task<List<T>> LoadAll<T>()
            where T: IEntity, new()
        {
            return Task.Run(() =>
            {
                var dummyInstance = new T();
                using (var dbContext = new SqliteDbContext())
                {
                    switch (dummyInstance)
                    {
                        case Ingredient ingredient:
                            return (List<T>)(object)dbContext.Ingredients.ToList();
                        case LogEvent logEvent:
                            return (List<T>)(object)dbContext.LogEvents.ToList();
                        case Recipe recipe:
                            return (List<T>)(object)dbContext.Recipes.ToList();
                        case SystemConfig system:
                            return (List<T>)(object)dbContext.SystemConfigs.ToList();
                        default:
                            throw new NotImplementedException(typeof(T).FullName);
                    }
                }
            });
        }

        public Task Save<T>(T instance)
            where T: IEntity
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    if (instance.Id != 0)
                    {
                        switch (instance)
                        {
                            case Ingredient ingredient:
                                dbContext.Ingredients.Update(ingredient);
                                break;
                            case LogEvent logEvent:
                                dbContext.LogEvents.Update(logEvent);
                                break;
                            case Recipe recipe:
                                dbContext.Recipes.Update(recipe);
                                break;
                            case SystemConfig system:
                                dbContext.SystemConfigs.Update(system);
                                break;
                            default:
                                throw new NotImplementedException(typeof(T).FullName);
                        }
                    }
                    else
                    {
                        switch (instance)
                        {
                            case Ingredient ingredient:
                                dbContext.Ingredients.Add(ingredient);
                                break;
                            case LogEvent logEvent:
                                dbContext.LogEvents.Add(logEvent);
                                break;
                            case Recipe recipe:
                                dbContext.Recipes.Add(recipe);
                                break;
                            case SystemConfig system:
                                dbContext.SystemConfigs.Add(system);
                                break;
                            default:
                                throw new NotImplementedException(typeof(T).FullName);
                        }
                    }
                    dbContext.SaveChanges();
                }
            });
        }

        public Task Delete<T>(T instance)
            where T : IEntity
        {
            return Task.Run(() =>
            {
                using (var dbContext = new SqliteDbContext())
                {
                    switch (instance)
                    {
                        case Ingredient ingredient:
                            dbContext.Ingredients.Remove(ingredient);
                            break;
                        case LogEvent logEvent:
                            dbContext.LogEvents.Remove(logEvent);
                            break;
                        case Recipe recipe:
                            dbContext.Recipes.Remove(recipe);
                            break;
                        case SystemConfig system:
                            dbContext.SystemConfigs.Remove(system);
                            break;
                        default:
                            throw new NotImplementedException(typeof(T).FullName);
                    }
                    dbContext.SaveChanges();
                }
            });
        }
    }
}
