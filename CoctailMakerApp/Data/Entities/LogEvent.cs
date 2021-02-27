using System;
using System.ComponentModel.DataAnnotations;

namespace CoctailMakerApp.Data.Entities
{
    public class LogEvent
    {
        [Key]
        public int Id { get; set; }
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
    }

    public enum LogEventType
    {
        Exception,
        Error,
        Warning,
        Info,
        Debug
    }
}
