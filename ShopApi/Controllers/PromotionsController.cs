using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
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

        // Tao chuong trinh khuyen mai.
        [HttpPost]
        public async Task<IActionResult> Create(PromotionDto dto)
        {
            var promo = await _service.CreateAsync(dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tao thanh cong",
                Data = promo
            });
        }

        // Gan khuyen mai vao san pham.
        [HttpPost("{promoId}/products/{productId}")]
        public async Task<IActionResult> Assign(int promoId, int productId)
        {
            var data = await _service.AssignAsync(promoId, productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Gan thanh cong",
                Data = data
            });
        }
    }
}

