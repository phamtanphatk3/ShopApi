using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Warranty;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/warranty")]
    public class WarrantyController : ControllerBase
    {
        private readonly WarrantyService _service;

        public WarrantyController(WarrantyService service)
        {
            _service = service;
        }

        // Tao phieu bao hanh moi (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWarrantyDto dto)
        {
            var data = await _service.CreateAsync(dto);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tao bao hanh thanh cong",
                Data = data
            });
        }

        // Tra cuu bao hanh theo serial, so dien thoai hoac ma don.
        [AllowAnonymous]
        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup(
            [FromQuery] string? serial,
            [FromQuery] string? phone,
            [FromQuery] int? orderId)
        {
            var data = await _service.LookupAsync(serial, phone, orderId);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }
    }
}

