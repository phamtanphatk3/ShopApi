namespace ShopApi.DTOs.Warranty
{
    public class CreateWarrantyDto
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int WarrantyMonths { get; set; } = 12;
    }
}
