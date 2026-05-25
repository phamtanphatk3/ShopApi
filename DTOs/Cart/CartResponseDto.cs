namespace ShopApi.DTOs.Cart
{
    // DTO trao doi du lieu CartResponseDto.
    public class CartResponseDto
    {
        public List<object> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }
    }
}

