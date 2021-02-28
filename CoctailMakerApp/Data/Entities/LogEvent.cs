using CoctailMakerApp.Lib;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoctailMakerApp.Data.Entities
{
    public class LogEvent : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public EventCode Code { get; set; }
        [Required]
        public LogEventType Type { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Func { get; set; }
        [Required]
        public string File { get; set; }
        [Required]
        public int Line { get; set; }
        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string DataAsJson
        {
            get { return JsonHelper.Serialize(Data); }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Data = JsonHelper.Deserialize<LogEventData>(value);
                }
                else
                {
                    Data = new LogEventData();
                }
            }
        }

        [NotMapped]
        public LogEventData Data { get; set; }
    }

    public class LogEventData
    {
        // Additional optional event data (no database column)
    }

    public enum LogEventType
    {
        Exception,
        Error,
        Warning,
        Info,
        Debug
    }

    public enum EventCode
    {
        UnknownEvent,
        FilesystemFailed,
    }
}
