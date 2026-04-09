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

        // ================= CREATE ORDER =================
        public async Task<OrderResponseDto> CreateOrder(string? couponCode = null)
        {
            // 🔥 LẤY USER ID
            var userIdClaim = _http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("Unauthorized");

            var userId = int.Parse(userIdClaim.Value);

            // 🔥 TRANSACTION (QUAN TRỌNG)
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 🔥 CART theo user hiện tại
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.Items.Any())
                    throw new Exception("Cart is empty");

                // ================= CHECK STOCK =================
                foreach (var item in cart.Items)
                {
                    if (item.Product == null)
                        throw new Exception("Product not found");

                    if (item.Product.StockQuantity < item.Quantity)
                        throw new Exception($"Product {item.Product.Name} out of stock");
                }

                // ================= COUPON =================
                Coupon? coupon = null;

                if (!string.IsNullOrEmpty(couponCode))
                {
                    coupon = await _context.Coupons
                        .FirstOrDefaultAsync(x => x.Code == couponCode);

                    if (coupon == null)
                        throw new Exception("Coupon not found");

                    if (coupon.ExpiryDate < DateTime.Now)
                        throw new Exception("Coupon expired");

                    if (coupon.UsedCount >= coupon.UsageLimit)
                        throw new Exception("Coupon usage limit reached");
                }

                // ================= CREATE ORDER =================
                var order = new Order
                {
                    UserId = userId,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    Items = new List<OrderItem>()
                };

                foreach (var item in cart.Items)
                {
                    order.Items.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price
                    });

                    // 🔥 TRỪ KHO
                    item.Product.StockQuantity -= item.Quantity;

                    // 🔥 INVENTORY LOG
                    _context.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductId = item.ProductId,
                        Quantity = -item.Quantity,
                        Type = "EXPORT",
                        CreatedAt = DateTime.Now
                    });
                }

                // ================= TÍNH TIỀN =================
                order.TotalPrice = order.Items.Sum(x => x.Price * x.Quantity);

                // ================= APPLY COUPON =================
                if (coupon != null)
                {
                    if (order.TotalPrice < coupon.MinOrderValue)
                        throw new Exception("Not enough order value for coupon");

                    if (coupon.DiscountPercent.HasValue)
                    {
                        order.TotalPrice *= (1 - coupon.DiscountPercent.Value / 100);
                    }
                    else if (coupon.DiscountAmount.HasValue)
                    {
                        order.TotalPrice -= coupon.DiscountAmount.Value;
                    }

                    // 🔥 KHÔNG CHO ÂM
                    if (order.TotalPrice < 0)
                        order.TotalPrice = 0;

                    coupon.UsedCount++;
                }

                // ================= SAVE =================
                _context.Orders.Add(order);

                // 🔥 CLEAR CART
                _context.CartItems.RemoveRange(cart.Items);

                await _context.SaveChangesAsync();

                // 🔥 COMMIT
                await transaction.CommitAsync();

                return new OrderResponseDto
                {
                    Id = order.Id,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    Items = order.Items.Select(x => new
                    {
                        x.ProductId,
                        x.Quantity,
                        x.Price
                    }).ToList<object>()
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // ================= UPDATE STATUS =================
        public async Task UpdateStatus(int orderId, string status)
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
        }
    }
}
