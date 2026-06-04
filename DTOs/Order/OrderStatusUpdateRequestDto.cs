using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Order
{
    public class OrderStatusUpdateRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Reason { get; set; }
    }
}
