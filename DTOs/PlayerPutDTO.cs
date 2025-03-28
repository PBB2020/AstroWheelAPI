namespace AstroWheelAPI.DTOs
{
    public class PlayerPutDTO
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int? IslandId { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
