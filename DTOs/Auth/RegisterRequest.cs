using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Auth
{
    // DTO dang ky tai khoan moi.
    public class RegisterRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Password { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? Address { get; set; }

        // Role tuy chon; neu de trong se mac dinh la Customer.
        public string? Role { get; set; }
    }
}
