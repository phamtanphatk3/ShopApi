namespace ShopApi.DTOs.Report
{
    // DTO trao doi du lieu LowStockProductDto.
    public class LowStockProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
    }
}

