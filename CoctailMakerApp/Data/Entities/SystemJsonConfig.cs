using CoctailMakerApp.Lib;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CoctailMakerApp.Data.Entities
{
    public class SystemConfig
    {
        [Range(0, 10)]
        [Required]
        public int MaxIngredients { get; set; }
        [Required]
        public int[] IngredientIds { get; set; }

        public void UpdateIngredientIds()
        {
            if (IngredientIds == null)
            {
                IngredientIds = new int[MaxIngredients];
            }
            else if (IngredientIds.Length != MaxIngredients)
            {
                var maxCopyCount = Math.Min(IngredientIds.Length, MaxIngredients);
                var newValues = new int[MaxIngredients];
                for (int i = 0; i < maxCopyCount; i++)
                {
                    newValues[i] = IngredientIds[i];
                }
                IngredientIds = newValues;
            }
        }
                
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
