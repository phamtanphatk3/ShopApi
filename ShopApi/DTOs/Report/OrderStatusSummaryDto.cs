namespace ShopApi.DTOs.Report
{
    public class OrderStatusSummaryDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
