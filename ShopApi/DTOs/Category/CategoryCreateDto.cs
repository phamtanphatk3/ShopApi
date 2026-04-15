namespace ShopApi.DTOs.Category
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public int? ParentCategoryId { get; set; }
    }

}
