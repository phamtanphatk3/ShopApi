namespace ShopApi.DTOs.Report
{
    // DTO trao doi du lieu OrderStatusSummaryDto.
    public class OrderStatusSummaryDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}

