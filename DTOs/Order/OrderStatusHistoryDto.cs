namespace ShopApi.DTOs.Order
{
    public class OrderStatusHistoryDto
    {
        public int Id { get; set; }
        public string? FromStatus { get; set; }
        public string ToStatus { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public string? ChangedByUsername { get; set; }
        public string? ChangedByRole { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
