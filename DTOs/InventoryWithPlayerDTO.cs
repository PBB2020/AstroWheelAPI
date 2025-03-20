using AstroWheelAPI.Models;

namespace AstroWheelAPI.DTOs
{
    public class InventoryWithPlayerDTO
    {
        public int InventoryId { get; set; }
        public int TotalScore { get; set; }
        public int PlayerId { get; set; }
        public string? PlayerName { get; set; }
    }
}
