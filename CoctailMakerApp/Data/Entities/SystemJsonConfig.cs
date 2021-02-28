using CoctailMakerApp.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CoctailMakerApp.Data.Entities
{
    public class SystemConfig
    {
        [Required]
        public List<int> IngredientIds { get; set; } = new List<int>();                        
        public int IntValue { get; set; }
        public float FloatValue { get; set; }
        public double DoubleValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public string StringValue { get; set; }
    }

    public class SystemJsonConfig
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string JsonContent { get; set; }

        [NotMapped]
        public SystemConfig Content
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(JsonContent))
                {
                    return JsonHelper.Deserialize<SystemConfig>(JsonContent);
                }

                return new SystemConfig();
            }
            set
            {
                JsonContent = JsonHelper.Serialize(value);
            }
        }
    }
}
