using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Product
{
    // DTO trao doi du lieu ProductCreateDto.
    public class ProductCreateDto
    {
        [Required]
        [StringLength(150, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string SKU { get; set; } = string.Empty;

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Brand { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999999")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }
}

