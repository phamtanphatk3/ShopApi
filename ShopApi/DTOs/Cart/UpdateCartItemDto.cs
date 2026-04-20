using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Cart
{
    // DTO trao doi du lieu UpdateCartItemDto.
    public class UpdateCartItemDto
    {
        [Range(1, 9999)]
        public int Quantity { get; set; }
    }
}

