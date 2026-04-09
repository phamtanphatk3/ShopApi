namespace ShopApi.Models
{
    public class InstallmentRequest
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal ProductPrice { get; set; }

        public int Months { get; set; } // so thang (3,6,12)
        public decimal DownPayment { get; set; } // tra truoc
        public decimal MonthlyPayment { get; set; } // tra moi thang

        public string CustomerName { get; set; }
        public string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
