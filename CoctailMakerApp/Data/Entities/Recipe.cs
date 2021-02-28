using CoctailMakerApp.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Entities
{
    public class Recipe : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }        
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Base64Image { get; set; }
        
        [Required]
        public string SettingsAsJson
        {
            get { return JsonHelper.Serialize(Settings); }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Settings = JsonHelper.Deserialize<RecipeSettings>(value);
                }
                else
                {
                    Settings = new RecipeSettings();
                }
            }
        }

        [NotMapped]
        public RecipeSettings Settings { get; set; }
    }

    public class RecipeSettings
    {
        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    }

    public class RecipeIngredient
    {
        public int IngredientId { get; set; }

        public int Quantity { get; set; }
    }
}
