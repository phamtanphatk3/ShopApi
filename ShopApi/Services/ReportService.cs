using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.DTOs.Report;

namespace ShopApi.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RevenueByDayDto>> GetRevenueByDay(DateTime? from, DateTime? to)
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
        }

        public async Task<List<RevenueByMonthDto>> GetRevenueByMonth(DateTime? from, DateTime? to)
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
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProducts(int top)
        {
            if (top <= 0) top = 5;

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
        }

        public async Task<List<OrderStatusSummaryDto>> GetOrderStatusSummary()
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
        }

        public async Task<List<LowStockProductDto>> GetLowStockProducts(int threshold)
        {
            if (threshold < 0) threshold = 0;

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
        }

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
    }
}
