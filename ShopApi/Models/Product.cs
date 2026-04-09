namespace ShopApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
        public List<ProductSpecification> Specifications { get; set; } = new();
        public List<ProductImage> Images { get; set; } = new();
        public List<ProductRegionPrice> RegionPrices { get; set; } = new();
    }
}
