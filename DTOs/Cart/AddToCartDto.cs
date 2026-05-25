using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Cart
{
    // DTO trao doi du lieu AddToCartDto.
    public class AddToCartDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, 9999)]
        public int Quantity { get; set; }
    }
}

