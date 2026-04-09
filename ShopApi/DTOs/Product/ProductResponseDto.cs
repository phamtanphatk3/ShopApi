namespace ShopApi.DTOs.Product
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
