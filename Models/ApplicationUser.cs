using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AstroWheelAPI.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        [StringLength(255)]
        public string PlayerName { get; set; } = string.Empty;
        public int PlayerLevel { get; set; } = 1;
        public int TotalScore { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
