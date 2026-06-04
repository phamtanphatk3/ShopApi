namespace ShopApi.DTOs.Order
{
    // DTO trao doi du lieu OrderResponseDto.
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal FinalAmount { get; set; }
        public string? CouponCode { get; set; }
        public string? CancelReason { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? LastStatusChangedAt { get; set; }

        public List<object> Items { get; set; } = new();
    }
}
