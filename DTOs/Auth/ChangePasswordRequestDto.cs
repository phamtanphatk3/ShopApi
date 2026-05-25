using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Auth
{
    public class ChangePasswordRequestDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "ConfirmPassword phai khop voi NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
