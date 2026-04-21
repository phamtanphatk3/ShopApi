using FluentValidation;
using ShopApi.DTOs.Installment;
using ShopApi.DTOs.Inventory;
using ShopApi.DTOs.Order;
using ShopApi.DTOs.ProductImage;

namespace ShopApi.Validators
{
    // Validator cho DTO InventoryDto.
    public class InventoryDtoValidator : AbstractValidator<InventoryDto>
    {
        public InventoryDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("So luong phai lon hon 0");
        }
    }

    // Validator cho DTO InstallmentCreateDto.
    public class InstallmentCreateDtoValidator : AbstractValidator<InstallmentCreateDto>
    {
        public InstallmentCreateDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId phai lon hon 0");

            RuleFor(x => x.Months)
                .InclusiveBetween(3, 36)
                .WithMessage("So thang tra gop phai trong khoang 3 den 36");

            RuleFor(x => x.DownPaymentPercent)
                .InclusiveBetween(0, 100)
                .WithMessage("Ty le tra truoc phai trong khoang 0 den 100");

            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .WithMessage("Ten khach hang la bat buoc")
                .Length(2, 100)
                .WithMessage("Ten khach hang phai tu 2 den 100 ky tu");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("So dien thoai la bat buoc")
                .Length(8, 20)
                .WithMessage("So dien thoai phai tu 8 den 20 ky tu");
        }
    }

    // Validator cho DTO CreateOrderRequestDto.
    public class CreateOrderRequestDtoValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderRequestDtoValidator()
        {
            RuleFor(x => x.CouponCode)
                .MaximumLength(50)
                .WithMessage("CouponCode khong duoc vuot qua 50 ky tu")
                .When(x => !string.IsNullOrWhiteSpace(x.CouponCode));
        }
    }

    // Validator cho DTO PromotionDto.
    public class PromotionDtoValidator : AbstractValidator<PromotionDto>
    {
        public PromotionDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ten khuyen mai la bat buoc")
                .Length(2, 100)
                .WithMessage("Ten khuyen mai phai tu 2 den 100 ky tu");

            RuleFor(x => x.DiscountType)
                .NotEmpty()
                .WithMessage("DiscountType la bat buoc")
                .Must(x => x == "Percent" || x == "Amount")
                .WithMessage("DiscountType chi duoc la Percent hoac Amount");

            RuleFor(x => x.DiscountValue)
                .NotNull()
                .WithMessage("DiscountValue la bat buoc")
                .GreaterThan(0)
                .WithMessage("DiscountValue phai lon hon 0");

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate!.Value)
                .WithMessage("EndDate phai lon hon hoac bang StartDate")
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
        }
    }
}
