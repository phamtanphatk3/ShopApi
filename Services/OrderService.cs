using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Order;
using ShopApi.Models;
using System.Security.Claims;

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
            var actor = GetActorContext();
            if (actor.UserId == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == actor.UserId.Value);

                if (cart == null || !cart.Items.Any())
                    throw new AppBadRequestException("Gio hang dang trong");

                foreach (var item in cart.Items)
                {
                    if (item.Product == null)
                        throw new AppNotFoundException("Khong tim thay san pham");

                    if (item.Product.StockQuantity < item.Quantity)
                        throw new AppBadRequestException($"San pham {item.Product.Name} khong du ton kho");
                }

                Coupon? coupon = null;

                if (!string.IsNullOrWhiteSpace(couponCode))
                {
                    coupon = await _context.Coupons
                        .FirstOrDefaultAsync(x => x.Code == couponCode);

                    if (coupon == null)
                        throw new AppNotFoundException("Khong tim thay ma giam gia");

                    if (coupon.StartDate > DateTime.UtcNow || coupon.EndDate < DateTime.UtcNow)
                        throw new AppBadRequestException("Ma giam gia da het han");

                    if (coupon.UsedCount >= coupon.UsageLimit)
                        throw new AppBadRequestException("Ma giam gia da het luot su dung");
                }

                var user = await _context.Users.FindAsync(actor.UserId.Value);
                if (user == null)
                    throw new AppNotFoundException("Khong tim thay nguoi dung");

                var order = new Order
                {
                    UserId = actor.UserId.Value,
                    CustomerName = user.Username,
                    OrderCode = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{actor.UserId.Value}",
                    CouponCode = couponCode?.Trim(),
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    LastStatusChangedAt = DateTime.UtcNow,
                    Items = new List<OrderItem>(),
                    StatusHistories = new List<OrderStatusHistory>()
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
                        CreatedAt = DateTime.UtcNow
                    });
                }

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

                order.StatusHistories.Add(new OrderStatusHistory
                {
                    FromStatus = null,
                    ToStatus = order.Status,
                    Reason = "Tao don hang",
                    ChangedByUserId = actor.UserId,
                    ChangedByRole = actor.Role,
                    ChangedByUsername = actor.Username,
                    ChangedAt = DateTime.UtcNow
                });

                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(cart.Items);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return MapOrderResponse(order);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Cap nhat trang thai don hang theo danh sach trang thai hop le.
        public async Task<object> UpdateStatus(int orderId, string status, string? reason = null)
        {
            var actor = GetActorContext();
            if (actor.UserId == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            var order = await _context.Orders
                .Include(x => x.StatusHistories)
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (order == null)
                throw new AppNotFoundException("Khong tim thay don hang");

            var validStatus = new[]
            {
                "Pending", "Confirmed", "Shipping", "Completed", "Cancelled"
            };

            if (!validStatus.Contains(status))
                throw new AppBadRequestException("Trang thai don hang khong hop le");

            var previousStatus = order.Status;
            order.Status = status;
            order.LastStatusChangedAt = DateTime.UtcNow;

            if (string.Equals(status, "Cancelled", StringComparison.OrdinalIgnoreCase))
            {
                order.CancelledAt = DateTime.UtcNow;
                order.CancelReason = string.IsNullOrWhiteSpace(reason) ? order.CancelReason : reason.Trim();
            }

            order.StatusHistories.Add(new OrderStatusHistory
            {
                FromStatus = previousStatus,
                ToStatus = status,
                Reason = string.IsNullOrWhiteSpace(reason)
                    ? $"Cap nhat trang thai sang {status}"
                    : reason.Trim(),
                ChangedByUserId = actor.UserId,
                ChangedByRole = actor.Role,
                ChangedByUsername = actor.Username,
                ChangedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return MapOrderSummary(order);
        }

        // Lay don hang cua user dang dang nhap.
        public async Task<object> GetMyOrders()
        {
            var actor = GetActorContext();
            if (actor.UserId == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            return await _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == actor.UserId.Value)
                .Select(o => new
                {
                    o.Id,
                    o.OrderCode,
                    o.CustomerName,
                    o.CouponCode,
                    o.CancelReason,
                    o.CancelledAt,
                    o.LastStatusChangedAt,
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
                    o.CouponCode,
                    o.CancelReason,
                    o.CancelledAt,
                    o.LastStatusChangedAt,
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
            var actor = GetActorContext();
            if (actor.UserId == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            var order = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.User)
                .Include(o => o.StatusHistories)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new AppNotFoundException("Khong tim thay don hang");

            if (string.Equals(actor.Role, "Customer", StringComparison.OrdinalIgnoreCase) && order.UserId != actor.UserId)
                throw new AppForbiddenException("Ban khong co quyen xem don hang nay");

            return MapOrderDetail(order);
        }

        public async Task<object> GetStatusHistoryAsync(int orderId)
        {
            var actor = GetActorContext();
            if (actor.UserId == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new AppNotFoundException("Khong tim thay don hang");

            if (string.Equals(actor.Role, "Customer", StringComparison.OrdinalIgnoreCase) && order.UserId != actor.UserId)
                throw new AppForbiddenException("Ban khong co quyen xem lich su don hang nay");

            return await _context.OrderStatusHistories
                .Where(x => x.OrderId == orderId)
                .OrderBy(x => x.ChangedAt)
                .Select(x => new OrderStatusHistoryDto
                {
                    Id = x.Id,
                    FromStatus = x.FromStatus,
                    ToStatus = x.ToStatus,
                    Reason = x.Reason,
                    ChangedByUsername = x.ChangedByUsername,
                    ChangedByRole = x.ChangedByRole,
                    ChangedAt = x.ChangedAt
                })
                .ToListAsync();
        }

        public async Task<object> CancelOrderAsync(int orderId, string? reason = null)
        {
            var actor = GetActorContext();
            if (actor.UserId == null)
                throw new AppUnauthorizedException("Chua dang nhap");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var order = await _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Include(o => o.StatusHistories)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                    throw new AppNotFoundException("Khong tim thay don hang");

                if (string.Equals(actor.Role, "Customer", StringComparison.OrdinalIgnoreCase) && order.UserId != actor.UserId)
                    throw new AppForbiddenException("Ban khong co quyen huy don hang nay");

                if (!string.Equals(order.Status, "Pending", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(order.Status, "Confirmed", StringComparison.OrdinalIgnoreCase))
                    throw new AppBadRequestException("Chi co the huy don Pending hoac Confirmed");

                var previousStatus = order.Status;
                var cancelReason = string.IsNullOrWhiteSpace(reason)
                    ? (string.Equals(actor.Role, "Customer", StringComparison.OrdinalIgnoreCase)
                        ? "Khach hang huy don"
                        : "Nguoi quan tri huy don")
                    : reason.Trim();

                order.Status = "Cancelled";
                order.CancelReason = cancelReason;
                order.CancelledAt = DateTime.UtcNow;
                order.LastStatusChangedAt = DateTime.UtcNow;

                foreach (var item in order.Items)
                {
                    item.Product.StockQuantity += item.Quantity;
                    _context.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Type = "IMPORT",
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (!string.IsNullOrWhiteSpace(order.CouponCode))
                {
                    var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.Code == order.CouponCode);
                    if (coupon != null && coupon.UsedCount > 0)
                        coupon.UsedCount--;
                }

                order.StatusHistories.Add(new OrderStatusHistory
                {
                    FromStatus = previousStatus,
                    ToStatus = "Cancelled",
                    Reason = cancelReason,
                    ChangedByUserId = actor.UserId,
                    ChangedByRole = actor.Role,
                    ChangedByUsername = actor.Username,
                    ChangedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new
                {
                    order.Id,
                    order.OrderCode,
                    order.Status,
                    order.CancelReason,
                    order.CancelledAt
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private static OrderResponseDto MapOrderResponse(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                Status = order.Status,
                FinalAmount = order.FinalAmount,
                CouponCode = order.CouponCode,
                CancelReason = order.CancelReason,
                CancelledAt = order.CancelledAt,
                LastStatusChangedAt = order.LastStatusChangedAt,
                Items = order.Items.Select(x => new
                {
                    x.ProductId,
                    x.Quantity,
                    x.UnitPrice,
                    x.LineTotal
                }).Cast<object>().ToList()
            };
        }

        private static object MapOrderSummary(Order order)
        {
            return new
            {
                order.Id,
                order.OrderCode,
                order.Status,
                order.CouponCode,
                order.CancelReason,
                order.CancelledAt,
                order.LastStatusChangedAt,
                order.FinalAmount,
                order.CreatedAt
            };
        }

        private static object MapOrderDetail(Order order)
        {
            return new
            {
                order.Id,
                order.OrderCode,
                order.CustomerName,
                order.CouponCode,
                order.CancelReason,
                order.CancelledAt,
                order.LastStatusChangedAt,
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
                }),
                StatusHistory = order.StatusHistories
                    .OrderBy(x => x.ChangedAt)
                    .Select(x => new OrderStatusHistoryDto
                    {
                        Id = x.Id,
                        FromStatus = x.FromStatus,
                        ToStatus = x.ToStatus,
                        Reason = x.Reason,
                        ChangedByUsername = x.ChangedByUsername,
                        ChangedByRole = x.ChangedByRole,
                        ChangedAt = x.ChangedAt
                    })
            };
        }

        private (int? UserId, string Role, string Username) GetActorContext()
        {
            var user = _http.HttpContext?.User;
            var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int? userId = int.TryParse(userIdClaim, out var parsedUserId) ? parsedUserId : null;

            return (
                userId,
                user?.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                user?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty
            );
        }
    }
}
