using ShopApi.Data;
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

        public async Task<object> Create(InstallmentCreateDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) throw new Exception("Product not found");

            var price = product.Price;

            // Tinh tra truoc
            var downPayment = price * dto.DownPaymentPercent / 100;

            // So tien con lai
            var remaining = price - downPayment;

            // Gop moi thang
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
