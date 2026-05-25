using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Order
{
    // DTO trao doi du lieu CreateOrderRequestDto.
    public class CreateOrderRequestDto
    {
        [StringLength(50)]
        public string? CouponCode { get; set; }
    }
}

