using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
