namespace ShopApi.DTOs.Category
{
    public class CategoryUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public bool IsActive { get; set; }
    }
}
