namespace ShopApi.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string Key { get; set; }     // VD: RAM
        public string Value { get; set; }   // VD: 8GB
    }
}
