using CoctailMakerApp.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CoctailMakerApp.Data.Entities
{
    public class SystemSettings
    {
        [Required]
        public List<int> IngredientIds { get; set; } = new List<int>();                        
        public int IntValue { get; set; }
        public float FloatValue { get; set; }
        public double DoubleValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public string StringValue { get; set; }
    }

    public class SystemConfig : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string SettingsAsJson
        {
            get { return JsonHelper.Serialize(Settings); }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Settings = JsonHelper.Deserialize<SystemSettings>(value);
                }
                else
                {
                    Settings = new SystemSettings();
                }
            }
        }

        [NotMapped]
        public SystemSettings Settings { get; set; } = new SystemSettings();
    }
}
