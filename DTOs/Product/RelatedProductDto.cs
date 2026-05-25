namespace ShopApi.DTOs.Product
{
    // DTO trao doi du lieu RelatedProductDto.
    public class RelatedProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

