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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Khong tim thay khuyen mai" });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PromotionUpdateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cap nhat thanh cong",
                Data = data
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Xoa thanh cong",
                Data = data
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

        [HttpDelete("{promoId}/products/{productId}")]
        public async Task<IActionResult> Unassign(int promoId, int productId)
        {
            var data = await _service.UnassignAsync(promoId, productId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Bo gan thanh cong",
                Data = data
            });
        }
    }
}
