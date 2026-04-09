namespace ShopApi.DTOs.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal FinalPrice { get; set; }

        public List<string> Images { get; set; }

        public List<ProductSpecificationDto> Specifications { get; set; }

        public List<RelatedProductDto> RelatedProducts { get; set; }
    }
}
