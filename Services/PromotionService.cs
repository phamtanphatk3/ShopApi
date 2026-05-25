using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.ProductImage;
using ShopApi.Models;

namespace ShopApi.Services
{
    public class PromotionService
    {
        private readonly AppDbContext _context;

        public PromotionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PromotionResponseDto>> GetAllAsync()
        {
            var promotions = await _context.Promotions
                .Include(x => x.ProductPromotions)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return promotions
                .Select(x => new PromotionResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    DiscountType = x.DiscountType,
                    DiscountValue = x.DiscountValue,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    ProductIds = x.ProductPromotions.Select(pp => pp.ProductId).ToList()
                })
                .ToList();
        }

        public async Task<PromotionResponseDto?> GetByIdAsync(int id)
        {
            var promo = await _context.Promotions
                .Where(x => x.Id == id)
                .Include(x => x.ProductPromotions)
                .FirstOrDefaultAsync();

            if (promo == null)
                return null;

            return new PromotionResponseDto
            {
                Id = promo.Id,
                Name = promo.Name,
                DiscountType = promo.DiscountType,
                DiscountValue = promo.DiscountValue,
                StartDate = promo.StartDate,
                EndDate = promo.EndDate,
                IsActive = promo.IsActive,
                ProductIds = promo.ProductPromotions.Select(pp => pp.ProductId).ToList()
            };
        }

        // Tao chuong trinh khuyen mai moi.
        public async Task<Promotion> CreateAsync(PromotionDto dto)
        {
            var discountType = dto.DiscountType;
            var discountValue = dto.DiscountValue;

            if (string.IsNullOrWhiteSpace(discountType) || !discountValue.HasValue)
                throw new AppBadRequestException("DiscountType va DiscountValue la bat buoc");

            var promo = new Promotion
            {
                Name = dto.Name,
                DiscountType = discountType,
                DiscountValue = discountValue.Value,
                StartDate = dto.StartDate ?? DateTime.Now,
                EndDate = dto.EndDate ?? DateTime.Now.AddDays(7),
                IsActive = true
            };

            _context.Promotions.Add(promo);
            await _context.SaveChangesAsync();

            return promo;
        }

        public async Task<PromotionResponseDto> UpdateAsync(int id, PromotionUpdateDto dto)
        {
            var promo = await _context.Promotions.FirstOrDefaultAsync(x => x.Id == id);
            if (promo == null)
                throw new AppNotFoundException("Khong tim thay khuyen mai");

            promo.Name = dto.Name;
            promo.DiscountType = dto.DiscountType;
            promo.DiscountValue = dto.DiscountValue;
            promo.StartDate = dto.StartDate;
            promo.EndDate = dto.EndDate;
            promo.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            var productIds = await _context.ProductPromotions
                .Where(x => x.PromotionId == promo.Id)
                .Select(x => x.ProductId)
                .ToListAsync();

            return new PromotionResponseDto
            {
                Id = promo.Id,
                Name = promo.Name,
                DiscountType = promo.DiscountType,
                DiscountValue = promo.DiscountValue,
                StartDate = promo.StartDate,
                EndDate = promo.EndDate,
                IsActive = promo.IsActive,
                ProductIds = productIds
            };
        }

        public async Task<object> DeleteAsync(int id)
        {
            var promo = await _context.Promotions
                .Include(x => x.ProductPromotions)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (promo == null)
                throw new AppNotFoundException("Khong tim thay khuyen mai");

            _context.ProductPromotions.RemoveRange(promo.ProductPromotions);
            _context.Promotions.Remove(promo);
            await _context.SaveChangesAsync();

            return new { promo.Id };
        }

        // Gan khuyen mai vao san pham.
        public async Task<object> AssignAsync(int promoId, int productId)
        {
            var promotionExists = await _context.Promotions.AnyAsync(x => x.Id == promoId);
            if (!promotionExists)
                throw new AppNotFoundException("Khong tim thay khuyen mai");

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new AppNotFoundException("Khong tim thay san pham");

            var alreadyAssigned = await _context.ProductPromotions
                .AnyAsync(x => x.PromotionId == promoId && x.ProductId == productId);
            if (alreadyAssigned)
                throw new AppConflictException("Khuyen mai da duoc gan cho san pham nay");

            var map = new ProductPromotion
            {
                ProductId = productId,
                PromotionId = promoId
            };

            _context.ProductPromotions.Add(map);
            await _context.SaveChangesAsync();

            return new
            {
                map.ProductId,
                map.PromotionId
            };
        }

        public async Task<object> UnassignAsync(int promoId, int productId)
        {
            var map = await _context.ProductPromotions
                .FirstOrDefaultAsync(x => x.PromotionId == promoId && x.ProductId == productId);

            if (map == null)
                throw new AppNotFoundException("Khong tim thay ket noi khuyen mai va san pham");

            _context.ProductPromotions.Remove(map);
            await _context.SaveChangesAsync();

            return new
            {
                map.ProductId,
                map.PromotionId
            };
        }
    }
}
