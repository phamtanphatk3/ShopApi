using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Coupon;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/coupons")]
    [Authorize]
    public class CouponsController : ControllerBase
    {
        private readonly CouponService _service;

        public CouponsController(CouponService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
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
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound(new ApiResponse<string> { Success = false, Message = "Khong tim thay ma giam gia" });

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] CouponCreateDto dto)
        {
            var data = await _service.CreateAsync(dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tao thanh cong",
                Data = data
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(int id, [FromBody] CouponUpdateDto dto)
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
        [Authorize(Roles = "Admin,Staff")]
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

        // Kiem tra coupon theo contract API toi thieu.
        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] string code, [FromQuery] decimal orderAmount = 0)
        {
            var data = await _service.ValidateAsync(code, orderAmount);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Ma giam gia hop le",
                Data = data
            });
        }
    }
}
