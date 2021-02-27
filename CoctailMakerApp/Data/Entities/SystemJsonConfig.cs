using CoctailMakerApp.Lib;
using System;
using System.ComponentModel.DataAnnotations;

namespace CoctailMakerApp.Data.Entities
{
    public class SystemJsonConfig
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string JsonContent { get; set; }

        public SystemConfig Config
        {
            set
            {
                JsonContent = JsonHelper.Serialize(value);
            }
        }

        public SystemConfig GetConfig()
        {
            if (!string.IsNullOrWhiteSpace(JsonContent))
            {
                return JsonHelper.Deserialize<SystemConfig>(JsonContent);
            }

            return new SystemConfig();
        }
    }
}
