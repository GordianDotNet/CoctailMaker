using System;
using System.ComponentModel.DataAnnotations;

namespace CoctailMakerApp.Data.Entities
{
    public class Ingredient
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
        public double Quantity { get; set; }
        [Required]
        public Unit QuantityUnit { get; set; }
    }

    public enum Unit
    {
        ml,
        g,
    }
}
