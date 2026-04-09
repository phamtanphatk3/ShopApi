namespace ShopApi.DTOs.Order
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }

        public List<object> Items { get; set; }
    }
}
