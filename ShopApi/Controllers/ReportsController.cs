using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/reports")]
    [Authorize(Roles = "Admin,Staff")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportService _service;

        public ReportsController(ReportService service)
        {
            _service = service;
        }

        [HttpGet("revenue/daily")]
        public async Task<IActionResult> RevenueByDay([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var data = await _service.GetRevenueByDay(from, to);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        [HttpGet("revenue/monthly")]
        public async Task<IActionResult> RevenueByMonth([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var data = await _service.GetRevenueByMonth(from, to);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        [HttpGet("top-selling-products")]
        public async Task<IActionResult> TopSellingProducts([FromQuery] int top = 5)
        {
            var data = await _service.GetTopSellingProducts(top);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        [HttpGet("orders-by-status")]
        public async Task<IActionResult> OrdersByStatus()
        {
            var data = await _service.GetOrderStatusSummary();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStock([FromQuery] int threshold = 5)
        {
            var data = await _service.GetLowStockProducts(threshold);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Success",
                Data = data
            });
        }
    }
}
