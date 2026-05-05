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

        // Bao cao doanh thu theo ngay.
        [HttpGet("revenue/daily")]
        public async Task<IActionResult> RevenueByDay([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var data = await _service.GetRevenueByDay(from, to);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Alias route theo contract API toi thieu: /api/reports/sales
        [HttpGet("sales")]
        public async Task<IActionResult> Sales(
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] string groupBy = "day")
        {
            if (groupBy.Equals("month", StringComparison.OrdinalIgnoreCase))
            {
                var monthData = await _service.GetRevenueByMonth(from, to);
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Thanh cong",
                    Data = monthData
                });
            }

            var dayData = await _service.GetRevenueByDay(from, to);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = dayData
            });
        }

        // Bao cao doanh thu theo thang.
        [HttpGet("revenue/monthly")]
        public async Task<IActionResult> RevenueByMonth([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var data = await _service.GetRevenueByMonth(from, to);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Lay top san pham ban chay.
        [HttpGet("top-selling-products")]
        public async Task<IActionResult> TopSellingProducts([FromQuery] int top = 5)
        {
            var data = await _service.GetTopSellingProducts(top);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Thong ke don hang theo trang thai.
        [HttpGet("orders-by-status")]
        public async Task<IActionResult> OrdersByStatus()
        {
            var data = await _service.GetOrderStatusSummary();
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }

        // Lay danh sach san pham sap het hang.
        [HttpGet("low-stock")]
        public async Task<IActionResult> LowStock([FromQuery] int threshold = 5)
        {
            var data = await _service.GetLowStockProducts(threshold);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }
    }
}

