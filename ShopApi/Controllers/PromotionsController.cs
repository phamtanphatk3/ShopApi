using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.DTOs.ProductImage;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/promotions")]
    [Authorize(Roles = "Admin,Staff")]
    public class PromotionsController : ControllerBase
    {
        private readonly PromotionService _service;

        public PromotionsController(PromotionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PromotionDto dto)
        {
            var promo = await _service.CreateAsync(dto);
            return Ok(promo);
        }

        [HttpPost("{promoId}/products/{productId}")]
        public async Task<IActionResult> Assign(int promoId, int productId)
        {
            await _service.AssignAsync(promoId, productId);
            return Ok("Gán thành công");
        }
    }
}
