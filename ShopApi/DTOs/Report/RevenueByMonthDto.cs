namespace ShopApi.DTOs.Report
{
    // DTO trao doi du lieu RevenueByMonthDto.
    public class RevenueByMonthDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}

