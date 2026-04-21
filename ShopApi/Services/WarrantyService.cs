using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
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

        // Tao ban ghi bao hanh moi cho san pham trong don hang.
        public async Task<WarrantyLookupResponseDto> CreateAsync(CreateWarrantyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SerialNumber))
                throw new AppBadRequestException("Serial la bat buoc");

            if (string.IsNullOrWhiteSpace(dto.CustomerPhone))
                throw new AppBadRequestException("So dien thoai khach hang la bat buoc");

            if (dto.WarrantyMonths <= 0)
                throw new AppBadRequestException("So thang bao hanh phai lon hon 0");

            var serial = dto.SerialNumber.Trim();
            var phone = dto.CustomerPhone.Trim();

            var orderExists = await _context.Orders.AnyAsync(x => x.Id == dto.OrderId);
            if (!orderExists)
                throw new AppNotFoundException("Khong tim thay don hang");

            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == dto.ProductId);
            if (product == null)
                throw new AppNotFoundException("Khong tim thay san pham");

            var orderHasProduct = await _context.OrderItems.AnyAsync(x =>
                x.OrderId == dto.OrderId && x.ProductId == dto.ProductId);
            if (!orderHasProduct)
                throw new AppBadRequestException("San pham khong thuoc don hang nay");

            var duplicateSerial = await _context.WarrantyRecords.AnyAsync(x => x.SerialNumber == serial);
            if (duplicateSerial)
                throw new AppConflictException("Serial da ton tai");

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

        // Tra cuu bao hanh theo serial, so dien thoai hoac ma don hang.
        public async Task<List<WarrantyLookupResponseDto>> LookupAsync(
            string? serial,
            string? phone,
            int? orderId)
        {
            if (string.IsNullOrWhiteSpace(serial) &&
                string.IsNullOrWhiteSpace(phone) &&
                !orderId.HasValue)
            {
                throw new AppBadRequestException("Can truyen it nhat mot dieu kien: serial, so dien thoai hoac ma don hang");
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

        // Chuyen doi du lieu WarrantyRecord sang DTO tra cuu.
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



