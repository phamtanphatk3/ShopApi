using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Store
{
    public class StoreCreateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Province { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string District { get; set; } = string.Empty;

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Address { get; set; } = string.Empty;

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }
    }
}
