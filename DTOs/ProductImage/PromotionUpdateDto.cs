using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.ProductImage
{
    public class PromotionUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Percent|Amount)$", ErrorMessage = "DiscountType chi duoc la Percent hoac Amount")]
        public string DiscountType { get; set; } = "Percent";

        [Required]
        [Range(typeof(decimal), "0.01", "999999999999")]
        public decimal DiscountValue { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
