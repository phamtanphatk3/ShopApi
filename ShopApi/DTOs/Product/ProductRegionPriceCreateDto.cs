namespace ShopApi.DTOs.Product
{
    public class ProductRegionPriceCreateDto
    {
        public string Region { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
