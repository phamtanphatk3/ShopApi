using FluentValidation;
using ShopApi.DTOs.Coupon;
using ShopApi.DTOs.ProductImage;
using ShopApi.DTOs.Store;

namespace ShopApi.Validators
{
    public class PromotionUpdateDtoValidator : AbstractValidator<PromotionUpdateDto>
    {
        public PromotionUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ten khuyen mai la bat buoc")
                .Length(2, 100).WithMessage("Ten khuyen mai phai tu 2 den 100 ky tu");

            RuleFor(x => x.DiscountType)
                .NotEmpty().WithMessage("DiscountType la bat buoc")
                .Must(x => x == "Percent" || x == "Amount")
                .WithMessage("DiscountType chi duoc la Percent hoac Amount");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("DiscountValue phai lon hon 0");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("EndDate phai lon hon StartDate");
        }
    }

    public class CouponCreateDtoValidator : AbstractValidator<CouponCreateDto>
    {
        public CouponCreateDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code la bat buoc")
                .Length(2, 50).WithMessage("Code phai tu 2 den 50 ky tu");

            RuleFor(x => x.DiscountType)
                .NotEmpty().WithMessage("DiscountType la bat buoc")
                .Must(x => x == "Percent" || x == "Amount")
                .WithMessage("DiscountType chi duoc la Percent hoac Amount");

            RuleFor(x => x.DiscountValue)
                .GreaterThan(0).WithMessage("DiscountValue phai lon hon 0");

            RuleFor(x => x.UsageLimit)
                .GreaterThan(0).WithMessage("UsageLimit phai lon hon 0");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("EndDate phai lon hon StartDate");
        }
    }

    public class CouponUpdateDtoValidator : AbstractValidator<CouponUpdateDto>
    {
        public CouponUpdateDtoValidator()
        {
            Include(new CouponCreateDtoValidator());
        }
    }

    public class StoreCreateDtoValidator : AbstractValidator<StoreCreateDto>
    {
        public StoreCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ten cua hang la bat buoc")
                .Length(2, 150).WithMessage("Ten cua hang phai tu 2 den 150 ky tu");

            RuleFor(x => x.Province)
                .NotEmpty().WithMessage("Tinh thanh la bat buoc")
                .Length(2, 100).WithMessage("Tinh thanh phai tu 2 den 100 ky tu");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("Quan/huyen la bat buoc")
                .Length(2, 100).WithMessage("Quan/huyen phai tu 2 den 100 ky tu");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Dia chi la bat buoc")
                .Length(5, 255).WithMessage("Dia chi phai tu 5 den 255 ky tu");
        }
    }

    public class StoreUpdateDtoValidator : AbstractValidator<StoreUpdateDto>
    {
        public StoreUpdateDtoValidator()
        {
            Include(new StoreCreateDtoValidator());
        }
    }
}
