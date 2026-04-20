using Microsoft.EntityFrameworkCore;
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
                throw new Exception("Unauthorized");

            var userId = int.Parse(userIdClaim.Value);

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.Items.Any())
                    throw new Exception("Cart is empty");

                // Kiem tra ton kho cho tung item truoc khi tao don.
                foreach (var item in cart.Items)
                {
                    if (item.Product == null)
                        throw new Exception("Product not found");

                    if (item.Product.StockQuantity < item.Quantity)
                        throw new Exception($"Product {item.Product.Name} out of stock");
                }

                // Kiem tra tinh hop le cua coupon neu co ap dung.
                Coupon? coupon = null;

                if (!string.IsNullOrWhiteSpace(couponCode))
                {
                    coupon = await _context.Coupons
                        .FirstOrDefaultAsync(x => x.Code == couponCode);

                    if (coupon == null)
                        throw new Exception("Coupon not found");

                    if (coupon.StartDate > DateTime.Now || coupon.EndDate < DateTime.Now)
                        throw new Exception("Coupon expired");

                    if (coupon.UsedCount >= coupon.UsageLimit)
                        throw new Exception("Coupon usage limit reached");
                }

                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                    throw new Exception("User not found");

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
                        throw new Exception("Not enough order value for coupon");

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
                throw new Exception("Order not found");

            var validStatus = new[]
            {
                "Pending", "Confirmed", "Shipping", "Completed", "Cancelled"
            };

            if (!validStatus.Contains(status))
                throw new Exception("Invalid status");

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
    }
}
