using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Common;
using ShopApi.DTOs.Order;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }

        // Tao don hang tu gio hang cua khach hang.
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestDto? dto)
        {
            var data = await _service.CreateOrder(dto?.CouponCode);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Tao thanh cong",
                Data = data
            });
        }

        // Cap nhat trang thai don hang (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var data = await _service.UpdateStatus(id, status);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Cap nhat thanh cong",
                Data = data
            });
        }

        // Lay danh sach don hang cua khach hang dang dang nhap.
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var orders = await _service.GetMyOrders();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = orders
            });
        }

        // Lay tat ca don hang (chi Admin/Staff).
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _service.GetAllOrders();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = orders
            });
        }

        // Lay chi tiet don hang theo id va kiem tra quyen truy cap.
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Thanh cong",
                Data = data
            });
        }
    }
}

