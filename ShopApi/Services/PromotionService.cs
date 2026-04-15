using Microsoft.EntityFrameworkCore;
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

        public async Task<Promotion> CreateAsync(PromotionDto dto)
        {
            var discountType = dto.DiscountType;
            var discountValue = dto.DiscountValue;

            if (string.IsNullOrWhiteSpace(discountType) || !discountValue.HasValue)
                throw new Exception("DiscountType and DiscountValue are required");

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

        public async Task AssignAsync(int promoId, int productId)
        {
            var promotionExists = await _context.Promotions.AnyAsync(x => x.Id == promoId);
            if (!promotionExists)
                throw new Exception("Promotion not found");

            var productExists = await _context.Products.AnyAsync(x => x.Id == productId);
            if (!productExists)
                throw new Exception("Product not found");

            var alreadyAssigned = await _context.ProductPromotions
                .AnyAsync(x => x.PromotionId == promoId && x.ProductId == productId);
            if (alreadyAssigned)
                throw new Exception("Promotion already assigned to product");

            var map = new ProductPromotion
            {
                ProductId = productId,
                PromotionId = promoId
            };

            _context.ProductPromotions.Add(map);
            await _context.SaveChangesAsync();
        }
    }
}
