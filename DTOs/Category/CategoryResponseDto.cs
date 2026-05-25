namespace ShopApi.DTOs.Category
{
    // DTO trao doi du lieu CategoryResponseDto.
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}

