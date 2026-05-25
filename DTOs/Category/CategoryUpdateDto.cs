using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.Category
{
    // DTO trao doi du lieu CategoryUpdateDto.
    public class CategoryUpdateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [StringLength(120)]
        public string? Slug { get; set; }

        public bool IsActive { get; set; }
    }
}

