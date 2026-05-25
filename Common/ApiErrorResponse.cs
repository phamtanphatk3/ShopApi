namespace ShopApi.Common
{
    // Dinh dang du lieu loi tra ve tu middleware exception.
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string? TraceId { get; set; }
        public string? Path { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
