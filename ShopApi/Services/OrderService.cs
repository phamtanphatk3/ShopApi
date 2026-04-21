using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Order;
using ShopApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ShopApi.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public OrderService(AppDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        // Tao don hang tu gio hang cua user, co xu ly coupon va tru ton kho.
        public async Task<OrderResponseDto> CreateOrder(string? couponCode = null)
        {
            var userIdClaim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            var userId = int.Parse(userIdClaim.Value);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.Items.Any())
                    throw new AppBadRequestException("Gio hang dang trong");

                // Kiem tra ton kho cho tung item truoc khi tao don.
                foreach (var item in cart.Items)
                {
                    if (item.Product == null)
                        throw new AppNotFoundException("Khong tim thay san pham");

                    if (item.Product.StockQuantity < item.Quantity)
                        throw new AppBadRequestException($"San pham {item.Product.Name} khong du ton kho");
                }

                // Kiem tra tinh hop le cua coupon neu co ap dung.
                Coupon? coupon = null;

                if (!string.IsNullOrWhiteSpace(couponCode))
                {
                    coupon = await _context.Coupons
                        .FirstOrDefaultAsync(x => x.Code == couponCode);

                    if (coupon == null)
                        throw new AppNotFoundException("Khong tim thay ma giam gia");

                    if (coupon.StartDate > DateTime.Now || coupon.EndDate < DateTime.Now)
                        throw new AppBadRequestException("Ma giam gia da het han");

                    if (coupon.UsedCount >= coupon.UsageLimit)
                        throw new AppBadRequestException("Ma giam gia da het luot su dung");
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new AppNotFoundException("Khong tim thay nguoi dung");

                // Tao don va tao danh sach order item tu cart item.
                var order = new Order
                {
                    UserId = userId,
                    CustomerName = user.Username,
                    OrderCode = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{userId}",
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    Items = new List<OrderItem>()
                };

                foreach (var item in cart.Items)
                {
                    var unitPrice = item.UnitPrice <= 0 ? item.Product.Price : item.UnitPrice;

                    order.Items.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        LineTotal = unitPrice * item.Quantity
                    });

                    item.Product.StockQuantity -= item.Quantity;

                    _context.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductId = item.ProductId,
                        Quantity = -item.Quantity,
                        Type = "EXPORT",
                        CreatedAt = DateTime.Now
                    });
                }

                // Tinh tong tien don va ap dung coupon neu hop le.
                order.FinalAmount = order.Items.Sum(x => x.LineTotal);

                if (coupon != null)
                {
                    if (order.FinalAmount < coupon.MinOrderValue)
                        throw new AppBadRequestException("Gia tri don hang chua dat muc toi thieu de ap ma");

                    if (coupon.DiscountType.Equals("Percent", StringComparison.OrdinalIgnoreCase))
                    {
                        order.FinalAmount *= (1 - coupon.DiscountValue / 100m);
                    }
                    else if (coupon.DiscountType.Equals("Amount", StringComparison.OrdinalIgnoreCase))
                    {
                        order.FinalAmount -= coupon.DiscountValue;
                    }

                    if (order.FinalAmount < 0)
                        order.FinalAmount = 0;

                    coupon.UsedCount++;
                }

                // Luu don, xoa gio hang va commit giao dich.
                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(cart.Items);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new OrderResponseDto
                {
                    Id = order.Id,
                    Status = order.Status,
                    FinalAmount = order.FinalAmount,
                    Items = order.Items.Select(x => new
                    {
                        x.ProductId,
                        x.Quantity,
                        x.UnitPrice,
                        x.LineTotal
                    }).ToList<object>()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Cap nhat trang thai don hang theo danh sach trang thai hop le.
        public async Task<object> UpdateStatus(int orderId, string status)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
                throw new AppNotFoundException("Khong tim thay don hang");

            var validStatus = new[]
            {
                "Pending", "Confirmed", "Shipping", "Completed", "Cancelled"
            };

            if (!validStatus.Contains(status))
                throw new AppBadRequestException("Trang thai don hang khong hop le");

            order.Status = status;
            await _context.SaveChangesAsync();

            return new
            {
                order.Id,
                order.OrderCode,
                order.Status,
                order.FinalAmount,
                order.CreatedAt
            };
        }

        // Lay don hang cua user dang dang nhap.
        public async Task<object> GetMyOrders()
        {
            var userIdClaim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            var userId = int.Parse(userIdClaim.Value);

            return await _context.Orders
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
        }

        // Lay tat ca don hang (cho Admin/Staff).
        public async Task<object> GetAllOrders()
        {
            return await _context.Orders
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
        }

        // Lay chi tiet don theo id va role dang dang nhap.
        public async Task<object> GetById(int orderId)
        {
            var userIdClaim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            var role = _http.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
            var userId = int.Parse(userIdClaim.Value);

            var order = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new AppNotFoundException("Khong tim thay don hang");

            if (string.Equals(role, "Customer", StringComparison.OrdinalIgnoreCase) && order.UserId != userId)
                throw new AppForbiddenException("Ban khong co quyen xem don hang nay");

            return new
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
            };
        }
    }
}


