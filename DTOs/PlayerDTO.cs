namespace AstroWheelAPI.DTOs
{
    public class PlayerDTO
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set;  } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int? CharacterId { get; set; }
        public int? IslandId { get; set; }
        public int? InventoryId { get; set; }
        public int? RecipeBookId { get; set; }
        public int TotalScore { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt  { get; set; }    
        public string? CharacterName { get; set; }
        public string? IslandName { get; set; }
        public List<PlayerMaterialDTO>? Materials { get; set; } // Hozzáadott sor
    }
}