namespace ShopApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public decimal FinalAmount { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
