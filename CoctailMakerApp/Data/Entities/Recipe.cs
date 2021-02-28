using CoctailMakerApp.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoctailMakerApp.Data.Entities
{
    public class Recipe
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
        public string JsonContent { get; set; }

        [NotMapped]
        public RecipeConfig Content
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(JsonContent))
                {
                    return JsonHelper.Deserialize<RecipeConfig>(JsonContent);
                }

                return new RecipeConfig();
            }
            set
            {
                JsonContent = JsonHelper.Serialize(value);
            }
        }
    }

    public class RecipeConfig
    {
        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    }

    public class RecipeIngredient
    {
        public int IngredientId { get; set; }

        public int Quantity { get; set; }
    }
}
