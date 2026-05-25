using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Product
{
    // DTO trao doi du lieu ProductRegionPriceCreateDto.
    public class ProductRegionPriceCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Region { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999999")]
        public decimal Price { get; set; }
    }
}

