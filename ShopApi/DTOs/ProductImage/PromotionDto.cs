namespace ShopApi.DTOs.ProductImage
{
    public class PromotionDto
    {
        public string Name { get; set; } = string.Empty;
        public string? DiscountType { get; set; }
        public decimal? DiscountValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
