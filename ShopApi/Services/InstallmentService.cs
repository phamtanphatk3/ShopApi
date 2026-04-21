using ShopApi.Data;
using ShopApi.Common.Exceptions;
using ShopApi.DTOs.Installment;
using ShopApi.Models;

namespace ShopApi.Services
{
    public class InstallmentService
    {
        private readonly AppDbContext _context;

        public InstallmentService(AppDbContext context)
        {
            _context = context;
        }

        // Tao yeu cau tra gop va tinh so tien tra truoc, tra hang thang.
        public async Task<object> Create(InstallmentCreateDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) throw new AppNotFoundException("Khong tim thay san pham");

            var price = product.Price;

            // Tinh so tien tra truoc.
            var downPayment = price * dto.DownPaymentPercent / 100;

            // Tinh so tien con lai sau khi tra truoc.
            var remaining = price - downPayment;

            // Tinh so tien tra moi thang.
            var monthly = remaining / dto.Months;

            var request = new InstallmentRequest
            {
                ProductId = dto.ProductId,
                ProductPrice = price,
                Months = dto.Months,
                DownPayment = downPayment,
                MonthlyPayment = monthly,
                CustomerName = dto.CustomerName,
                Phone = dto.Phone
            };

            _context.InstallmentRequests.Add(request);
            await _context.SaveChangesAsync();

            return new
            {
                price,
                downPayment,
                monthly,
                months = dto.Months
            };
        }
    }
}



