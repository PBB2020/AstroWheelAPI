using System.ComponentModel.DataAnnotations;

namespace AstroWheelAPI.Models
{
    public class RecipeBook
    {
        [Key]
        public int RecipeId { get; set; }
        [Required]
        [StringLength(255)]
        public string RecipeName { get; set; } = string.Empty;
        [Required]
        public string Acquired { get; set; } = string.Empty;
    }
}
