namespace ShopApi.DTOs.Report
{
    public class RevenueByMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}
