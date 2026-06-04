namespace ShopApi.Models
{
    // Mo hinh du lieu Order.
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CouponCode { get; set; }
        public string? CancelReason { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastStatusChangedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal FinalAmount { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public List<OrderStatusHistory> StatusHistories { get; set; } = new();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
