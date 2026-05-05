using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/warranties")]
    [Authorize]
    public class WarrantiesController : ControllerBase
    {
        private readonly WarrantyService _service;

        public WarrantiesController(WarrantyService service)
        {
            _service = service;
        }

        // Alias route theo contract API toi thieu: /api/warranties/search
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? serial,
            [FromQuery] string? phone,
            [FromQuery] string? orderCode)
        {
            var data = await _service.LookupByOrderCodeAsync(serial, phone, orderCode);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }
    }
}
