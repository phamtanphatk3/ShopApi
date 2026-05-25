namespace ShopApi.DTOs.Report
{
    // DTO trao doi du lieu RevenueByDayDto.
    public class RevenueByDayDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}

