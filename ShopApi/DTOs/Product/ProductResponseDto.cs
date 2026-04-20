namespace ShopApi.DTOs.Product
{
    // DTO trao doi du lieu ProductResponseDto.
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}

