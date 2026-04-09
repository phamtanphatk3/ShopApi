namespace ShopApi.DTOs.Installment
{
    public class InstallmentCreateDto
    {
        public int ProductId { get; set; }
        public int Months { get; set; }
        public decimal DownPaymentPercent { get; set; } // ví dụ 20%
        public string CustomerName { get; set; }
        public string Phone { get; set; }
    }
}
