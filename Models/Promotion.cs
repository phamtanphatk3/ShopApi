namespace ShopApi.Models
{
    // Mo hinh du lieu Promotion.
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string DiscountType { get; set; } = "Percent";
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<ProductPromotion> ProductPromotions { get; set; } = new();
    }
}

