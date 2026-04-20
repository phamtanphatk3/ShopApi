using System.ComponentModel.DataAnnotations;

namespace ShopApi.DTOs.ProductImage
{
    // DTO trao doi du lieu UploadImageDto.
    public class UploadImageDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;

        public bool IsMain { get; set; }

        [Range(0, 9999)]
        public int SortOrder { get; set; }
    }
}

