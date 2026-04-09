namespace ShopApi.DTOs.Cart
{
    public class CartResponseDto
    {
        public List<object> Items { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
