namespace ShopApi.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }
        public string? ReplacedByTokenHash { get; set; }
        public string? CreatedByIp { get; set; }
        public string? RevokedByIp { get; set; }
        public bool IsActive => UsedAt == null && RevokedAt == null && ExpiresAt > DateTime.UtcNow;
    }
}
