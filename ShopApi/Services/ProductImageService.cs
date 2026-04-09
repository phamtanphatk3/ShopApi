using ShopApi.Data;
using ShopApi.Models;
using Microsoft.EntityFrameworkCore;
using ShopApi.DTOs.ProductImage;

namespace ShopApi.Services
{
    public class ProductImageService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductImageService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ✅ Upload nhiều ảnh + xử lý ảnh đại diện
        public async Task UploadAsync(int productId, UploadImageDto dto)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
            var path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // 🔥 Nếu là ảnh đại diện → tắt tất cả ảnh cũ
            if (dto.IsPrimary)
            {
                var oldImages = await _context.ProductImages
                    .Where(x => x.ProductId == productId)
                    .ToListAsync();

                foreach (var img in oldImages)
                {
                    img.IsPrimary = false;
                }
            }

            var image = new ProductImage
            {
                ImageUrl = "/images/" + fileName,
                ProductId = productId,
                IsPrimary = dto.IsPrimary,
                SortOrder = dto.SortOrder
            };

            _context.ProductImages.Add(image);
            await _context.SaveChangesAsync();
        }

        // ✅ Lấy danh sách ảnh (có sort)
        public async Task<List<ProductImageResponseDto>> GetImagesAsync(int productId)
        {
            return await _context.ProductImages
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.SortOrder)
                .Select(x => new ProductImageResponseDto
                {
                    ImageUrl = x.ImageUrl,
                    IsPrimary = x.IsPrimary,
                    SortOrder = x.SortOrder
                })
                .ToListAsync();
        }
    }
    
}
