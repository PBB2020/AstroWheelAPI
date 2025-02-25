using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AstroWheelAPI.Models
{
    public class Island
    {
        [Key]
        public int IslandId { get; set; }
        [Required]
        [StringLength(255)]
        public string IslandName { get; set; } = string.Empty;
        [Required]
        [StringLength(150)]
        public required string Element { get; set; }
        [JsonIgnore]
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}
