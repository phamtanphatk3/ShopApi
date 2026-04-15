namespace ShopApi.DTOs.Cart
{
    public class CartResponseDto
    {
        public List<object> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }
    }
}
