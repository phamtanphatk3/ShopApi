using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.Models;

namespace ShopApi.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        // Nhap kho
        public async Task ImportAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new AppBadRequestException("So luong phai lon hon 0");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new AppNotFoundException("Khong tim thay san pham");

            product.StockQuantity += quantity;

            var log = new InventoryTransaction
            {
                ProductId = productId,
                Quantity = quantity,
                Type = "IMPORT"
            };

            _context.InventoryTransactions.Add(log);
            await _context.SaveChangesAsync();
        }

        // Xuat kho
        public async Task ExportAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new AppBadRequestException("So luong phai lon hon 0");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new AppNotFoundException("Khong tim thay san pham");

            if (product.StockQuantity < quantity)
                throw new AppBadRequestException("Khong du hang trong kho");

            product.StockQuantity -= quantity;

            var log = new InventoryTransaction
            {
                ProductId = productId,
                Quantity = -quantity,
                Type = "EXPORT"
            };

            _context.InventoryTransactions.Add(log);
            await _context.SaveChangesAsync();
        }

        // Lich su giao dich kho
        public async Task<List<InventoryTransaction>> GetHistory(int productId)
        {
            return await _context.InventoryTransactions
                .Where(x => x.ProductId == productId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // Lay ton kho hien tai cua san pham.
        public async Task<object> GetStockAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new AppNotFoundException("Khong tim thay san pham");

            return new
            {
                product.Id,
                product.Name,
                product.StockQuantity
            };
        }
    }
}

