namespace ShopApi.Models
{
    public class OrderStatusHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string? FromStatus { get; set; }
        public string ToStatus { get; set; } = string.Empty;
        public string? Reason { get; set; }
        public int? ChangedByUserId { get; set; }
        public User? ChangedByUser { get; set; }
        public string? ChangedByRole { get; set; }
        public string? ChangedByUsername { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}
