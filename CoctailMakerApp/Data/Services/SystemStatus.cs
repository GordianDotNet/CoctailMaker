using System;

namespace CoctailMakerApp.Data.Services
{
    public record SystemStatus
    {
        public DateTime SystemTime { get; set; }
        public bool Led1State { get; set; }
    }
}
