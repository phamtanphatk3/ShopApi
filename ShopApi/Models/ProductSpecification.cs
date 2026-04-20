namespace ShopApi.Models
{
    // Mo hinh du lieu ProductSpecification.
    public class ProductSpecification
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public string Key { get; set; } = string.Empty;     // VD: RAM
        public string Value { get; set; } = string.Empty;   // VD: 8GB
    }
}

