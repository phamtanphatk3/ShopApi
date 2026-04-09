namespace ShopApi.DTOs.ProductImage
{
    public class UploadImageDto
    {
        public IFormFile File { get; set; }
        public bool IsPrimary { get; set; }
        public int SortOrder { get; set; }
    }
}
