namespace ShopApi.DTOs.ProductImage
{
    public class PromotionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DiscountType { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
