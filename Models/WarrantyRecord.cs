namespace ShopApi.Models
{
    // Mo hinh du lieu WarrantyRecord.
    public class WarrantyRecord
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public DateTime WarrantyStartDate { get; set; }
        public DateTime WarrantyEndDate { get; set; }
        public string Status { get; set; } = "InWarranty";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

