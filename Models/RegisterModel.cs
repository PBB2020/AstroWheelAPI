﻿namespace AstroWheelAPI.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PlayerName { get; set; } = string.Empty;
        public int CharacterId { get; set; }
    }
}
