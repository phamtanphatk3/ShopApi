using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data;
using ShopApi.DTOs.ProductImage;
using ShopApi.Models;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/promotions")]
    [Authorize(Roles = "Admin,Staff")]
    public class PromotionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PromotionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromotionDto dto)
        {
            var promo = new Promotion
            {
                Name = dto.Name,
                DiscountPercent = dto.DiscountPercent,
                DiscountAmount = dto.DiscountAmount,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                IsActive = true
            };

            _context.Promotions.Add(promo);
            await _context.SaveChangesAsync();

            return Ok(promo);
        }

        [HttpPost("{promoId}/products/{productId}")]
        public async Task<IActionResult> Assign(int promoId, int productId)
        {
            var map = new ProductPromotion
            {
                ProductId = productId,
                PromotionId = promoId
            };

            _context.ProductPromotions.Add(map);
            await _context.SaveChangesAsync();

            return Ok("Gán thành công");
        }
    }
}
