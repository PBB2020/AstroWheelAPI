using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstroWheelAPI.Models
{
    public class InventoryMaterial
    {
        // Összetett kulcs (InventoryId és MaterialId)
        [Key, Column(Order = 0)]
        [Required]
        public int InventoryId { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public int MaterialId { get; set; }
        //Navigációs tulajdonság az Inventory-hoz
        [ForeignKey(nameof(InventoryId))]
        public Inventory Inventory { get; set; } = null!;
        //Navigációs tulajdonság a Material-hoz
        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; } = null!;
        //Mennyiség, amennyi az adott anyagból van
        [Required]
        public int Quantity { get; set; }
    }
}
