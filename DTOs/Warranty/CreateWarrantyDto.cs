using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Warranty
{
    // DTO trao doi du lieu CreateWarrantyDto.
    public class CreateWarrantyDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20, MinimumLength = 8)]
        public string CustomerPhone { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int OrderId { get; set; }

        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(1, 120)]
        public int WarrantyMonths { get; set; } = 12;
    }
}

