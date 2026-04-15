namespace ShopApi.DTOs.Warranty
{
    public class WarrantyLookupResponseDto
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
