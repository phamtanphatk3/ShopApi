using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Order
{
    public class CancelOrderRequestDto
    {
        [MaxLength(255)]
        public string? Reason { get; set; }
    }
}
