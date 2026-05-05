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

        // Role tuy chon; neu de trong se mac dinh la Customer.
        public string? Role { get; set; }
    }
}
