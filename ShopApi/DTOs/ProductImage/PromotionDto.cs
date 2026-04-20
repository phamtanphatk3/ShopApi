using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.ProductImage
{
    // DTO trao doi du lieu PromotionDto.
    public class PromotionDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Percent|Amount)$", ErrorMessage = "DiscountType must be Percent or Amount")]
        public string? DiscountType { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "999999999999")]
        public decimal? DiscountValue { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

