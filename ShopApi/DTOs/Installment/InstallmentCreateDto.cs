using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Installment
{
    // DTO trao doi du lieu InstallmentCreateDto.
    public class InstallmentCreateDto
    {
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Range(3, 36)]
        public int Months { get; set; }

        [Range(typeof(decimal), "0", "100")]
        public decimal DownPaymentPercent { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20, MinimumLength = 8)]
        public string Phone { get; set; } = string.Empty;
    }
}

