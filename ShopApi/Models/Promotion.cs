namespace ShopApi.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public List<ProductPromotion> ProductPromotions { get; set; }
    }
}
