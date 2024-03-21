using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class CreateRegionRequestDTO
    {
        [Required]
        [StringLength(3,
            MinimumLength = 2,
            ErrorMessage ="{0} length must be between {2} and {1} characters")]
        public string Code { get; set; } = null!;
        [Required]
        [StringLength(100,
            MinimumLength = 3,
            ErrorMessage = "{0} length must be between {2} and {1} characters")]
        public string Name { get; set; } = null!;
        public string? RegionImageUrl { get; set; }
    }
}
