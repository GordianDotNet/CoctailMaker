using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}
