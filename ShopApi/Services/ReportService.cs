using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShopApi.Data;
using ShopApi.DTOs.Report;

namespace ShopApi.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public ReportService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Bao cao doanh thu theo ngay trong khoang thoi gian.
        public async Task<List<RevenueByDayDto>> GetRevenueByDay(DateTime? from, DateTime? to)
        {
            var cacheKey = $"reports:revenue:day:{from:yyyyMMdd}:{to:yyyyMMdd}";
            return await GetOrCreateAsync(cacheKey, async () =>
            {
                var query = _context.Orders.AsQueryable();
                query = ApplyDateRange(query, from, to);

                return await query
                    .GroupBy(o => o.CreatedAt.Date)
                    .Select(g => new RevenueByDayDto
                    {
                        Date = g.Key,
                        Revenue = g.Sum(x => x.FinalAmount),
                        OrderCount = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToListAsync();
            });
        }

        // Bao cao doanh thu theo thang trong khoang thoi gian.
        public async Task<List<RevenueByMonthDto>> GetRevenueByMonth(DateTime? from, DateTime? to)
        {
            var cacheKey = $"reports:revenue:month:{from:yyyyMMdd}:{to:yyyyMMdd}";
            return await GetOrCreateAsync(cacheKey, async () =>
            {
                var query = _context.Orders.AsQueryable();
                query = ApplyDateRange(query, from, to);

                return await query
                    .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
                    .Select(g => new RevenueByMonthDto
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        Revenue = g.Sum(x => x.FinalAmount),
                        OrderCount = g.Count()
                    })
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .ToListAsync();
            });
        }

        // Lay danh sach top san pham ban chay.
        public async Task<List<TopSellingProductDto>> GetTopSellingProducts(int top)
        {
            if (top <= 0) top = 5;

            var cacheKey = $"reports:top-selling:{top}";
            return await GetOrCreateAsync(cacheKey, async () =>
            {
                var validStatuses = new[] { "Confirmed", "Shipping", "Completed" };

                return await _context.OrderItems
                    .Where(oi => validStatuses.Contains(oi.Order.Status))
                    .GroupBy(oi => new { oi.ProductId, oi.Product.Name, oi.Product.SKU })
                    .Select(g => new TopSellingProductDto
                    {
                        ProductId = g.Key.ProductId,
                        ProductName = g.Key.Name,
                        SKU = g.Key.SKU,
                        QuantitySold = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.LineTotal)
                    })
                    .OrderByDescending(x => x.QuantitySold)
                    .ThenByDescending(x => x.Revenue)
                    .Take(top)
                    .ToListAsync();
            });
        }

        // Thong ke so luong don va doanh thu theo trang thai.
        public async Task<List<OrderStatusSummaryDto>> GetOrderStatusSummary()
        {
            const string cacheKey = "reports:orders-by-status";
            return await GetOrCreateAsync(cacheKey, async () =>
            {
                return await _context.Orders
                    .GroupBy(o => o.Status)
                    .Select(g => new OrderStatusSummaryDto
                    {
                        Status = g.Key,
                        Count = g.Count(),
                        TotalRevenue = g.Sum(x => x.FinalAmount)
                    })
                    .OrderBy(x => x.Status)
                    .ToListAsync();
            });
        }

        // Lay danh sach san pham ton kho thap theo nguong.
        public async Task<List<LowStockProductDto>> GetLowStockProducts(int threshold)
        {
            if (threshold < 0) threshold = 0;

            var cacheKey = $"reports:low-stock:{threshold}";
            return await GetOrCreateAsync(cacheKey, async () =>
            {
                return await _context.Products
                    .Where(p => p.StockQuantity <= threshold && p.IsActive)
                    .OrderBy(p => p.StockQuantity)
                    .ThenBy(p => p.Name)
                    .Select(p => new LowStockProductDto
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        SKU = p.SKU,
                        StockQuantity = p.StockQuantity
                    })
                    .ToListAsync();
            });
        }

        // Ap dung bo loc ngay bat dau/ket thuc cho truy van don hang.
        private static IQueryable<Models.Order> ApplyDateRange(
            IQueryable<Models.Order> query,
            DateTime? from,
            DateTime? to)
        {
            if (from.HasValue)
                query = query.Where(o => o.CreatedAt >= from.Value.Date);

            if (to.HasValue)
            {
                var toExclusive = to.Value.Date.AddDays(1);
                query = query.Where(o => o.CreatedAt < toExclusive);
            }

            return query;
        }

        // Cache bao cao trong 2 phut de giam tai truy van DB.
        private Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory)
        {
            return _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
                return await factory();
            })!;
        }
    }
}
