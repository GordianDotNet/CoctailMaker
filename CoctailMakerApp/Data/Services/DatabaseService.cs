using CoctailMakerApp.Data.Context;
using CoctailMakerApp.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
                try
                {
                    if (File.Exists(SqliteDbContext.DATABASE_FILENAME))
                    {
                        File.Delete(SqliteDbContext.DATABASE_FILENAME);
                    }
                    SaveLogEvent($"[Deleted] Database - '{Path.GetFullPath(SqliteDbContext.DATABASE_FILENAME)}'", EventCode.DatabaseDeleted, LogEventType.Warning);
                }
                catch (Exception ex)
                {
                    SaveLogEvent($"[Deleted] Database - '{Path.GetFullPath(SqliteDbContext.DATABASE_FILENAME)}'", EventCode.DatabaseDeleted, ex);
                }
            });
        }

        public Task<SystemConfig> LoadSystemConfigOrCreateDefault()
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var dbContext = new SqliteDbContext())
                    {
                        return dbContext.SystemConfigs.OrderByDescending(x => x.Id).FirstOrDefault() ?? new SystemConfig();
                    }
                }
                catch (Exception ex)
                {
                    SaveLogEvent($"{nameof(SystemConfig)}", EventCode.DatabaseEntityLoadingFailed, ex);
                }

                return new SystemConfig();
            });
        }

        public Task SaveLogEvent(string message, EventCode code, Exception ex, [CallerMemberName] string funcName = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            return Save(new LogEvent
            {
                Code = code,
                Type = LogEventType.Exception,
                Message = string.IsNullOrWhiteSpace(message) ? ex.Message : message,
                Func = funcName,
                File = file,
                Line = line,
                Data = new LogEventData { ExceptionData = new ExceptionData(ex) }
            });
        }

        public Task SaveLogEvent(string message, EventCode code, LogEventType logEventType = LogEventType.Debug, [CallerMemberName] string funcName = "", [CallerFilePath] string file = "", [CallerLineNumber] int line = 0)
        {
            return Save(new LogEvent
            {
                Code = code,
                Type = logEventType,
                Message = message,
                Func = funcName,
                File = file,
                Line = line
            });
        }

        public event Action<LogEvent> OnLogEventCreated;
        public Task LogEventCreated(LogEvent logEvent)
        {
            return Task.Run(() => OnLogEventCreated?.Invoke(logEvent));
        }

        public Task<List<T>> LoadAll<T>()
            where T : IEntity, new()
        {
            return Task.Run(() =>
            {
                try
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
                }
                catch (Exception ex)
                {
                    SaveLogEvent($"{typeof(T).Name}", EventCode.DatabaseEntityLoadingFailed, ex);
                }

                return new List<T>();
            });
        }

        public Task Save<T>(T instance)
            where T : IEntity
        {
            return Task.Run(() =>
            {
                var newInstance = instance.Id == 0;
                var modificationText = newInstance ? "[Added]" : "[Changed]";
                try
                {
                    using (var dbContext = new SqliteDbContext())
                    {
                        if (newInstance)
                        {
                            switch (instance)
                            {
                                case Ingredient ingredient:
                                    dbContext.Ingredients.Add(ingredient);
                                    break;
                                case LogEvent logEvent:
                                    dbContext.LogEvents.Add(logEvent);
                                    LogEventCreated(logEvent);
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
                        else
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
                        dbContext.SaveChanges();
                    }

                    var code = newInstance ? EventCode.DatabaseEntityAdded : EventCode.DatabaseEntityUpdated;
                    switch (instance)
                    {
                        case Ingredient ingredient:
                            SaveLogEvent($"{modificationText} {nameof(Ingredient)} - '{ingredient.Name}' ({ingredient.Id})", code, LogEventType.Info);
                            break;
                        case LogEvent logEvent:
                            // Don't log an LogEvent here again!
                            break;
                        case Recipe recipe:
                            SaveLogEvent($"{modificationText} {nameof(Recipe)} - '{recipe.Name}' ({recipe.Id})", code, LogEventType.Info);
                            break;
                        case SystemConfig system:
                            SaveLogEvent($"{modificationText} {nameof(SystemConfig)} - ({system.Id})", code, LogEventType.Info);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    var code = newInstance ? EventCode.DatabaseEntityAddedFailed : EventCode.DatabaseEntityUpdatedFailed;
                    SaveLogEvent($"{modificationText} {typeof(T).Name} - {ex?.Message} - {ex?.InnerException?.Message}", code, ex);
                }
            });
        }

        public Task Delete<T>(T instance)
            where T : IEntity
        {
            return Task.Run(() =>
            {
                var modificationText = "[Deleted]";

                try
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

                    var code = EventCode.DatabaseEntityDeleted;
                    switch (instance)
                    {
                        case Ingredient ingredient:
                            SaveLogEvent($"{modificationText} - {nameof(Ingredient)} - '{ingredient.Name}' ({ingredient.Id})", code, LogEventType.Info);
                            break;
                        case LogEvent logEvent:
                            // Don't log an LogEvent here again!
                            break;
                        case Recipe recipe:
                            SaveLogEvent($"{modificationText} - {nameof(Recipe)} - '{recipe.Name}' ({recipe.Id})", code, LogEventType.Info);
                            break;
                        case SystemConfig system:
                            SaveLogEvent($"{modificationText} - {nameof(SystemConfig)} - ({system.Id})", code, LogEventType.Info);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    var code = EventCode.DatabaseEntityDeletedFailed;
                    SaveLogEvent($"{modificationText} - {typeof(T).Name} - {ex?.Message} - {ex?.InnerException?.Message}", code, ex);
                }
            });
        }
    }
}
