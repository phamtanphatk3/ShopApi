using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;

namespace ShopApi.Services
{
    public class CouponService
    {
        private readonly AppDbContext _context;

        public CouponService(AppDbContext context)
        {
            _context = context;
        }

        // Kiem tra coupon va tinh gia tri giam cho don hang.
        public async Task<object> ValidateAsync(string code, decimal orderAmount)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new AppBadRequestException("Code la bat buoc");

            var normalizedCode = code.Trim();
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(x => x.Code == normalizedCode);

            if (coupon == null)
                throw new AppNotFoundException("Khong tim thay ma giam gia");

            var now = DateTime.UtcNow;
            if (coupon.StartDate > now || coupon.EndDate < now)
                throw new AppBadRequestException("Ma giam gia da het han");

            if (coupon.UsedCount >= coupon.UsageLimit)
                throw new AppBadRequestException("Ma giam gia da het luot su dung");

            if (orderAmount < coupon.MinOrderValue)
                throw new AppBadRequestException("Gia tri don hang chua dat muc toi thieu de ap ma");

            decimal discount = 0;
            if (coupon.DiscountType.Equals("Percent", StringComparison.OrdinalIgnoreCase))
                discount = orderAmount * coupon.DiscountValue / 100m;
            else if (coupon.DiscountType.Equals("Amount", StringComparison.OrdinalIgnoreCase))
                discount = coupon.DiscountValue;

            var finalAmount = orderAmount - discount;
            if (finalAmount < 0) finalAmount = 0;

            return new
            {
                code = coupon.Code,
                discountType = coupon.DiscountType,
                discountValue = coupon.DiscountValue,
                minOrderValue = coupon.MinOrderValue,
                orderAmount,
                discountAmount = discount,
                finalAmount
            };
        }
    }
}
