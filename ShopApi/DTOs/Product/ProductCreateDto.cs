namespace ShopApi.DTOs.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
    
}
