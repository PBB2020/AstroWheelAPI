namespace AstroWheelAPI.DTOs
{
    public class PlayerDTOPUT
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int? IslandId { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
