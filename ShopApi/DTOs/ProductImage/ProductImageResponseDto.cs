namespace ShopApi.DTOs.ProductImage
{
    // DTO trao doi du lieu ProductImageResponseDto.
    public class ProductImageResponseDto
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public int SortOrder { get; set; }
    }
}   


