namespace ShopApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Pending";

        public decimal TotalPrice { get; set; }

        public List<OrderItem> Items { get; set; } = new();

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
