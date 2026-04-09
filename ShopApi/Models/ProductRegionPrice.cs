namespace ShopApi.Models
{
    public class ProductRegionPrice
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Region { get; set; } // VD: "HCM", "HN"

        public decimal Price { get; set; }
    }
}
