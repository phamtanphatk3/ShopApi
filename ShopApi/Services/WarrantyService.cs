using Microsoft.EntityFrameworkCore;
using ShopApi.Data;
using ShopApi.DTOs.Warranty;
using ShopApi.Models;

namespace ShopApi.Services
{
    public class WarrantyService
    {
        private readonly AppDbContext _context;

        public WarrantyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WarrantyLookupResponseDto> CreateAsync(CreateWarrantyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SerialNumber))
                throw new Exception("Serial number is required");

            if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
                throw new Exception("Customer phone is required");

            if (dto.WarrantyMonths <= 0)
                throw new Exception("Warranty months must be greater than 0");

            var serial = dto.SerialNumber.Trim();
            var phone = dto.CustomerPhone.Trim();

            var orderExists = await _context.Orders.AnyAsync(x => x.Id == dto.OrderId);
            if (!orderExists)
                throw new Exception("Order not found");

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var orderHasProduct = await _context.OrderItems.AnyAsync(x =>
                x.OrderId == dto.OrderId && x.ProductId == dto.ProductId);
            if (!orderHasProduct)
                throw new Exception("Product does not belong to this order");

            var duplicateSerial = await _context.WarrantyRecords.AnyAsync(x => x.SerialNumber == serial);
            if (duplicateSerial)
                throw new Exception("Serial number already exists");

            var now = DateTime.UtcNow;
            var record = new WarrantyRecord
            {
                SerialNumber = serial,
                CustomerPhone = phone,
                OrderId = dto.OrderId,
                ProductId = dto.ProductId,
                WarrantyStartDate = now,
                WarrantyEndDate = now.AddMonths(dto.WarrantyMonths),
                Status = "InWarranty"
            };

            _context.WarrantyRecords.Add(record);
            await _context.SaveChangesAsync();

            return ToLookupDto(record, product.Name);
        }

        public async Task<List<WarrantyLookupResponseDto>> LookupAsync(
            string? serial,
            string? phone,
            int? orderId)
        {
            if (string.IsNullOrWhiteSpace(serial) &&
                string.IsNullOrWhiteSpace(phone) &&
                !orderId.HasValue)
            {
                throw new Exception("Provide at least one filter: serial, phone, or orderId");
            }

            var query = _context.WarrantyRecords
                .Include(x => x.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(serial))
            {
                var s = serial.Trim();
                query = query.Where(x => x.SerialNumber == s);
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                var p = phone.Trim();
                query = query.Where(x => x.CustomerPhone == p);
            }

            if (orderId.HasValue)
                query = query.Where(x => x.OrderId == orderId.Value);

            var data = await query
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var now = DateTime.UtcNow;
            var hasStatusChanged = false;
            foreach (var item in data)
            {
                var expectedStatus = item.WarrantyEndDate >= now ? "InWarranty" : "Expired";
                if (!string.Equals(item.Status, expectedStatus, StringComparison.Ordinal))
                {
                    item.Status = expectedStatus;
                    hasStatusChanged = true;
                }
            }

            if (hasStatusChanged)
                await _context.SaveChangesAsync();

            return data
                .Select(x => ToLookupDto(x, x.Product.Name))
                .ToList();
        }

        private static WarrantyLookupResponseDto ToLookupDto(WarrantyRecord record, string productName)
        {
            var now = DateTime.UtcNow;
            var status = record.WarrantyEndDate >= now ? "InWarranty" : "Expired";

            return new WarrantyLookupResponseDto
            {
                SerialNumber = record.SerialNumber,
                CustomerPhone = record.CustomerPhone,
                OrderId = record.OrderId,
                ProductId = record.ProductId,
                ProductName = productName,
                WarrantyStartDate = record.WarrantyStartDate,
                WarrantyEndDate = record.WarrantyEndDate,
                Status = status
            };
        }
    }
}
