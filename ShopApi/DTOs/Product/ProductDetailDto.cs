namespace ShopApi.DTOs.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }

        public List<string> Images { get; set; } = new();

        public List<ProductSpecificationDto> Specifications { get; set; } = new();

        public List<RelatedProductDto> RelatedProducts { get; set; } = new();
    }
}
