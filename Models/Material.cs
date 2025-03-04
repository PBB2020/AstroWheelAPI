using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AstroWheelAPI.Models
{
    [Index(nameof(EnglishName), IsUnique = true)]
    public class Material
    {
        [Key]
        public int MaterialId { get; set; } 
        [Required]
        [StringLength(100)]
        public string WitchName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string EnglishName { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        public string LatinName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<InventoryMaterial> InventoryMaterials { get; set; } = new List<InventoryMaterial>();
    }
}
