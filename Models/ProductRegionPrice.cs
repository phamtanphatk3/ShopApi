namespace ShopApi.Models
{
    // Mo hinh du lieu ProductRegionPrice.
    public class ProductRegionPrice
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string Region { get; set; } = string.Empty; // VD: "HCM", "HN"

        public decimal Price { get; set; }
    }
}

