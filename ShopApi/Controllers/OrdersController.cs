using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.DTOs.Order;
using ShopApi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize] // 🔥 tất cả API đều cần login
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;
        private readonly AppDbContext _context;

        public OrdersController(OrderService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        // ================= CREATE ORDER =================
        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestDto? dto)
        {
            var data = await _service.CreateOrder(dto?.CouponCode);
            return Ok(data);
        }

        // ================= UPDATE STATUS =================
        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            await _service.UpdateStatus(id, status);

            return Ok(new
            {
                message = "Updated"
            });
        }

        // ================= CUSTOMER XEM ĐƠN CỦA MÌNH =================
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var orders = await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .Select(o => new
                {
                    o.Id,
                    o.OrderCode,
                    o.CustomerName,
                    o.Status,
                    o.FinalAmount,
                    o.CreatedAt,
                    Items = o.Items.Select(i => new
                    {
                        i.ProductId,
                        i.Quantity,
                        i.UnitPrice,
                        i.LineTotal
                    })
                })
                .ToListAsync();

            return Ok(orders);
        }

        // ================= ADMIN / STAFF XEM TẤT CẢ =================
        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Select(o => new
                {
                    o.Id,
                    o.OrderCode,
                    o.CustomerName,
                    o.Status,
                    o.FinalAmount,
                    o.CreatedAt,
                    Username = o.User.Username
                })
                .ToListAsync();

            return Ok(orders);
        }

        // ================= XEM CHI TIẾT =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var order = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound("Order not found");

            // 🔥 CUSTOMER chỉ được xem đơn của mình
            if (role == "Customer" && order.UserId != userId)
                return Forbid();

            return Ok(new
            {
                order.Id,
                order.OrderCode,
                order.CustomerName,
                order.Status,
                order.FinalAmount,
                order.CreatedAt,
                Username = order.User.Username,
                Items = order.Items.Select(i => new
                {
                    i.ProductId,
                    i.Quantity,
                    i.UnitPrice,
                    i.LineTotal
                })
            });
        }
    }
}
