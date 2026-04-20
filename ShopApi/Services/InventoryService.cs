using ShopApi.Data;
using ShopApi.Models;
using Microsoft.EntityFrameworkCore;

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
                throw new Exception("Quantity must be greater than 0");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product không tồn tại");

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
                throw new Exception("Quantity must be greater than 0");

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                throw new Exception("Product không tồn tại");

            if (product.StockQuantity < quantity)
                throw new Exception("Không đủ hàng trong kho");

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
                throw new Exception("Product không tồn tại");

            return new
            {
                product.Id,
                product.Name,
                product.StockQuantity
            };
        }
    }
}
