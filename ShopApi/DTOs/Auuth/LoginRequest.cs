using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Auuth
{
    // DTO trao doi du lieu LoginRequest.
    public class LoginRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; } = string.Empty;
    }
}

