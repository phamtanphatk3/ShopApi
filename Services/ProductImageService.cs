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

        // Upload nhieu anh va xu ly anh dai dien
        public async Task UploadAsync(int productId, UploadImageDto dto)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
            var path = Path.Combine(_env.WebRootPath, "images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Neu la anh dai dien thi tat tat ca anh cu
            if (dto.IsMain)
            {
                var oldImages = await _context.ProductImages
                    .Where(x => x.ProductId == productId)
                    .ToListAsync();

                foreach (var img in oldImages)
                {
                    img.IsMain = false;
                }
            }

            var image = new ProductImage
            {
                ImageUrl = "/images/" + fileName,
                ProductId = productId,
                IsMain = dto.IsMain,
                SortOrder = dto.SortOrder
            };

            _context.ProductImages.Add(image);
            await _context.SaveChangesAsync();
        }

        // Lay danh sach anh theo thu tu sort
        public async Task<List<ProductImageResponseDto>> GetImagesAsync(int productId)
        {
            return await _context.ProductImages
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.SortOrder)
                .Select(x => new ProductImageResponseDto
                {
                    ImageUrl = x.ImageUrl,
                    IsMain = x.IsMain,
                    SortOrder = x.SortOrder
                })
                .ToListAsync();
        }

        public async Task DeleteAsync(int productId, int imageId)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.Id == imageId);

            if (image == null)
                throw new ShopApi.Common.Exceptions.AppNotFoundException("Khong tim thay anh san pham");

            var filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task SetMainAsync(int productId, int imageId)
        {
            var images = await _context.ProductImages
                .Where(x => x.ProductId == productId)
                .ToListAsync();

            var selected = images.FirstOrDefault(x => x.Id == imageId);
            if (selected == null)
                throw new ShopApi.Common.Exceptions.AppNotFoundException("Khong tim thay anh san pham");

            foreach (var image in images)
            {
                image.IsMain = image.Id == imageId;
            }

            await _context.SaveChangesAsync();
        }
    }
    
}
