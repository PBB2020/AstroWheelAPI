using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AstroWheelAPI.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        [Required]
        public int TotalScore { get; set; }
        [JsonIgnore]
        public ICollection<InventoryMaterial> InventoryMaterials { get; set; } = new List<InventoryMaterial>();
    }
}
