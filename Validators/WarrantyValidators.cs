using FluentValidation;
using ShopApi.DTOs.Warranty;

namespace ShopApi.Validators
{
    // Validator cho DTO CreateWarrantyDto.
    public class CreateWarrantyDtoValidator : AbstractValidator<CreateWarrantyDto>
    {
        public CreateWarrantyDtoValidator()
        {
            RuleFor(x => x.SerialNumber)
                .NotEmpty()
                .WithMessage("SerialNumber la bat buoc")
                .Length(3, 100)
                .WithMessage("SerialNumber phai tu 3 den 100 ky tu");

            RuleFor(x => x.CustomerPhone)
                .NotEmpty()
                .WithMessage("CustomerPhone la bat buoc")
                .Length(8, 20)
                .WithMessage("CustomerPhone phai tu 8 den 20 ky tu");

            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("OrderId phai lon hon 0");

            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId phai lon hon 0");

            RuleFor(x => x.WarrantyMonths)
                .InclusiveBetween(1, 120)
                .WithMessage("WarrantyMonths phai trong khoang 1 den 120");
        }
    }
}
