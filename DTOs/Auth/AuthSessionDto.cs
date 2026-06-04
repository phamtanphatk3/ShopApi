namespace ShopApi.DTOs.Auth
{
    public class AuthSessionDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiresAt { get; set; }
        public UserProfileDto User { get; set; } = new();
    }
}
