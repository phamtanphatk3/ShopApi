using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Category
{
    // DTO trao doi du lieu CategoryCreateDto.
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(120)]
        public string? Slug { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ParentCategoryId must be greater than 0")]
        public int? ParentCategoryId { get; set; }
    }
}

