using FluentValidation;
using ShopApi.DTOs.Product;

namespace ShopApi.Validators
{
    // Validator cho DTO ProductCreateDto.
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ten san pham la bat buoc")
                .Length(2, 150)
                .WithMessage("Ten san pham phai tu 2 den 150 ky tu");

            RuleFor(x => x.SKU)
                .NotEmpty()
                .WithMessage("SKU la bat buoc")
                .Length(3, 50)
                .WithMessage("SKU phai tu 3 den 50 ky tu");

            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Thuong hieu la bat buoc")
                .Length(2, 60)
                .WithMessage("Thuong hieu phai tu 2 den 60 ky tu");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Gia phai lon hon 0");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("So luong ton kho khong duoc am");

            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Mo ta khong duoc vuot qua 1000 ky tu");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId phai lon hon 0");
        }
    }

    // Validator cho DTO ProductUpdateDto.
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            Include(new ProductCreateDtoValidator());
        }
    }

    // Validator cho DTO ProductRegionPriceCreateDto.
    public class ProductRegionPriceCreateDtoValidator : AbstractValidator<ProductRegionPriceCreateDto>
    {
        public ProductRegionPriceCreateDtoValidator()
        {
            RuleFor(x => x.Region)
                .NotEmpty()
                .WithMessage("Region la bat buoc")
                .Length(2, 50)
                .WithMessage("Region phai tu 2 den 50 ky tu");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Gia khu vuc phai lon hon 0");
        }
    }

    // Validator cho DTO ProductRegionPriceUpdateDto.
    public class ProductRegionPriceUpdateDtoValidator : AbstractValidator<ProductRegionPriceUpdateDto>
    {
        public ProductRegionPriceUpdateDtoValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Gia khu vuc phai lon hon 0");
        }
    }
}
