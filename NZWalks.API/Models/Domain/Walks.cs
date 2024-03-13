using System.ComponentModel.DataAnnotations.Schema;

namespace NZWalks.API.Models.Domain
{
    public class Walks
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }


        public double LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid RegionId { get; set; }
        [ForeignKey("RegionId")]
        public Region Region { get; set; }

        public Guid DifficultyId { get; set; }
        [ForeignKey("DifficultyId")]

        public Difficulty Difficulty { get; set; }

    }
}
