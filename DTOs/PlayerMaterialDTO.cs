namespace AstroWheelAPI.DTOs
{
    public class PlayerMaterialDTO
    {
        public int MaterialId { get; set; }
        public string WitchName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string LatinName { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}