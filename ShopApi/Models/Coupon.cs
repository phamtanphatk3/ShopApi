namespace ShopApi.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string Code { get; set; }

        public decimal? DiscountPercent { get; set; }
        public decimal? DiscountAmount { get; set; }

        public decimal MinOrderValue { get; set; }

        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
