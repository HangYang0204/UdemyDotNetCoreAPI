using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minimal length should be 3 charactors")]
        [MaxLength(3, ErrorMessage = "Maximum length should be 3 charactors")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }//nullable
    }
}
