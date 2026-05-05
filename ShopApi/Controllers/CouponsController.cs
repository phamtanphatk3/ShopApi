using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
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
