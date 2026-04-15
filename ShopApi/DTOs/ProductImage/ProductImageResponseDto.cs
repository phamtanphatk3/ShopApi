namespace ShopApi.DTOs.ProductImage
{
    public class ProductImageResponseDto
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public int SortOrder { get; set; }
    }
}   

