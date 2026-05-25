using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Coupon
{
    public class CouponCreateDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Code { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(Percent|Amount)$", ErrorMessage = "DiscountType chi duoc la Percent hoac Amount")]
        public string DiscountType { get; set; } = "Percent";

        [Required]
        [Range(typeof(decimal), "0.01", "999999999999")]
        public decimal DiscountValue { get; set; }

        [Range(typeof(decimal), "0", "999999999999")]
        public decimal MinOrderValue { get; set; }

        [Range(1, int.MaxValue)]
        public int UsageLimit { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
