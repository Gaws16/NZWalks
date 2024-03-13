namespace NZWalks.API.Models.DTO
{
    public class CreateRegionRequestDTO
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
