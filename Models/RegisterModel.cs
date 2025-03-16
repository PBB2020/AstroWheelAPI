using System.ComponentModel.DataAnnotations;

namespace AstroWheelAPI.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string PlayerName { get; set; } = string.Empty;
        public int CharacterId { get; set; }
    }
}
