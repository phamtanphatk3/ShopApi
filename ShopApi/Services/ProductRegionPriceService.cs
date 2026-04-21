using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Product;
using ShopApi.Models;

namespace ShopApi.Services
{
    // Xu ly nghiep vu gia theo khu vuc cho san pham.
    public class ProductRegionPriceService
    {
        private readonly AppDbContext _context;

        public ProductRegionPriceService(AppDbContext context)
        {
            _context = context;
        }

        // Lay danh sach gia khu vuc theo productId.
        public async Task<object> GetByProduct(int productId)
        {
            await EnsureProductExists(productId);

            return await _context.ProductRegionPrices
                .Where(x => x.ProductId == productId)
                .OrderBy(x => x.Region)
                .Select(x => new
                {
                    x.Id,
                    x.Region,
                    x.Price
                })
                .ToListAsync();
        }

        // Tao moi gia khu vuc.
        public async Task<object> Create(int productId, ProductRegionPriceCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Region))
                throw new AppBadRequestException("Vung gia la bat buoc");
            if (dto.Price <= 0)
                throw new AppBadRequestException("Gia khu vuc phai lon hon 0");

            await EnsureProductExists(productId);

            var region = dto.Region.Trim();
            var existed = await _context.ProductRegionPrices.AnyAsync(x =>
                x.ProductId == productId &&
                x.Region.ToLower() == region.ToLower());
            if (existed)
                throw new AppConflictException("Gia khu vuc nay da ton tai cho san pham");

            var item = new ProductRegionPrice
            {
                ProductId = productId,
                Region = region,
                Price = dto.Price
            };

            _context.ProductRegionPrices.Add(item);
            await _context.SaveChangesAsync();

            return new { item.Id, item.ProductId, item.Region, item.Price };
        }

        // Cap nhat gia khu vuc theo region.
        public async Task<object> Update(int productId, string region, ProductRegionPriceUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(region))
                throw new AppBadRequestException("Vung gia la bat buoc");
            if (dto.Price <= 0)
                throw new AppBadRequestException("Gia khu vuc phai lon hon 0");

            var item = await _context.ProductRegionPrices.FirstOrDefaultAsync(x =>
                x.ProductId == productId &&
                x.Region.ToLower() == region.ToLower());
            if (item == null)
                throw new AppNotFoundException("Khong tim thay gia khu vuc");

            item.Price = dto.Price;
            await _context.SaveChangesAsync();

            return new { item.Id, item.ProductId, item.Region, item.Price };
        }

        // Xoa gia khu vuc theo region.
        public async Task<object> Delete(int productId, string region)
        {
            if (string.IsNullOrWhiteSpace(region))
                throw new AppBadRequestException("Vung gia la bat buoc");

            var item = await _context.ProductRegionPrices.FirstOrDefaultAsync(x =>
                x.ProductId == productId &&
                x.Region.ToLower() == region.ToLower());
            if (item == null)
                throw new AppNotFoundException("Khong tim thay gia khu vuc");

            _context.ProductRegionPrices.Remove(item);
            await _context.SaveChangesAsync();

            return new { item.Id, item.ProductId, item.Region };
        }

        // Kiem tra san pham ton tai.
        private async Task EnsureProductExists(int productId)
        {
            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new AppNotFoundException("Khong tim thay san pham");
        }
    }
}


