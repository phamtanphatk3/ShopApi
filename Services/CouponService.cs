using Microsoft.EntityFrameworkCore;
using ShopApi.Common.Exceptions;
using ShopApi.Data;
using ShopApi.DTOs.Coupon;
using ShopApi.Models;

namespace ShopApi.Services
{
    public class CouponService
    {
        private readonly AppDbContext _context;

        public CouponService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CouponResponseDto>> GetAllAsync()
        {
            return await _context.Coupons
                .OrderByDescending(x => x.Id)
                .Select(x => new CouponResponseDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    DiscountType = x.DiscountType,
                    DiscountValue = x.DiscountValue,
                    MinOrderValue = x.MinOrderValue,
                    UsageLimit = x.UsageLimit,
                    UsedCount = x.UsedCount,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .ToListAsync();
        }

        public async Task<CouponResponseDto?> GetByIdAsync(int id)
        {
            return await _context.Coupons
                .Where(x => x.Id == id)
                .Select(x => new CouponResponseDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    DiscountType = x.DiscountType,
                    DiscountValue = x.DiscountValue,
                    MinOrderValue = x.MinOrderValue,
                    UsageLimit = x.UsageLimit,
                    UsedCount = x.UsedCount,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CouponResponseDto> CreateAsync(CouponCreateDto dto)
        {
            var code = dto.Code.Trim();
            var existed = await _context.Coupons.AnyAsync(x => x.Code == code);
            if (existed)
                throw new AppConflictException("Ma giam gia da ton tai");

            var coupon = new Coupon
            {
                Code = code,
                DiscountType = dto.DiscountType.Trim(),
                DiscountValue = dto.DiscountValue,
                MinOrderValue = dto.MinOrderValue,
                UsageLimit = dto.UsageLimit,
                UsedCount = 0,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();

            return new CouponResponseDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                MinOrderValue = coupon.MinOrderValue,
                UsageLimit = coupon.UsageLimit,
                UsedCount = coupon.UsedCount,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate
            };
        }

        public async Task<CouponResponseDto> UpdateAsync(int id, CouponUpdateDto dto)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.Id == id);
            if (coupon == null)
                throw new AppNotFoundException("Khong tim thay ma giam gia");

            var code = dto.Code.Trim();
            var duplicated = await _context.Coupons.AnyAsync(x => x.Code == code && x.Id != id);
            if (duplicated)
                throw new AppConflictException("Ma giam gia da ton tai");

            coupon.Code = code;
            coupon.DiscountType = dto.DiscountType.Trim();
            coupon.DiscountValue = dto.DiscountValue;
            coupon.MinOrderValue = dto.MinOrderValue;
            coupon.UsageLimit = dto.UsageLimit;
            coupon.StartDate = dto.StartDate;
            coupon.EndDate = dto.EndDate;

            await _context.SaveChangesAsync();

            return new CouponResponseDto
            {
                Id = coupon.Id,
                Code = coupon.Code,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                MinOrderValue = coupon.MinOrderValue,
                UsageLimit = coupon.UsageLimit,
                UsedCount = coupon.UsedCount,
                StartDate = coupon.StartDate,
                EndDate = coupon.EndDate
            };
        }

        public async Task<object> DeleteAsync(int id)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(x => x.Id == id);
            if (coupon == null)
                throw new AppNotFoundException("Khong tim thay ma giam gia");

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();

            return new { coupon.Id };
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
