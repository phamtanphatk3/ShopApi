using FluentValidation;
using ShopApi.DTOs.Auuth;
using ShopApi.DTOs.Cart;
using ShopApi.DTOs.Category;
using ShopApi.DTOs.Installment;
using ShopApi.DTOs.Inventory;
using ShopApi.DTOs.Order;
using ShopApi.DTOs.Product;
using ShopApi.DTOs.ProductImage;
using ShopApi.DTOs.Warranty;

namespace ShopApi.Validators
{
    // Validator cho DTO LoginRequest.
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(3, 50);

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(3, 100);
        }
    }
    // Validator cho DTO CategoryCreateDto.
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Slug)
                .MaximumLength(120);

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0)
                .When(x => x.ParentCategoryId.HasValue);
        }
    }
    // Validator cho DTO CategoryUpdateDto.
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Slug)
                .MaximumLength(120);
        }
    }
    // Validator cho DTO ProductCreateDto.
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 150);

            RuleFor(x => x.SKU)
                .NotEmpty()
                .Length(3, 50);

            RuleFor(x => x.Brand)
                .NotEmpty()
                .Length(2, 60);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);
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
    // Validator cho DTO AddToCartDto.
    public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 9999);
        }
    }
    // Validator cho DTO UpdateCartItemDto.
    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 9999);
        }
    }
    // Validator cho DTO InventoryDto.
    public class InventoryDtoValidator : AbstractValidator<InventoryDto>
    {
        public InventoryDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
    // Validator cho DTO InstallmentCreateDto.
    public class InstallmentCreateDtoValidator : AbstractValidator<InstallmentCreateDto>
    {
        public InstallmentCreateDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Months)
                .InclusiveBetween(3, 36);

            RuleFor(x => x.DownPaymentPercent)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.CustomerName)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Length(8, 20);
        }
    }
    // Validator cho DTO CreateOrderRequestDto.
    public class CreateOrderRequestDtoValidator : AbstractValidator<CreateOrderRequestDto>
    {
        public CreateOrderRequestDtoValidator()
        {
            RuleFor(x => x.CouponCode)
                .MaximumLength(50)
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
                .Length(2, 100);

            RuleFor(x => x.DiscountType)
                .NotEmpty()
                .Must(x => x == "Percent" || x == "Amount")
                .WithMessage("DiscountType must be Percent or Amount");

            RuleFor(x => x.DiscountValue)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.EndDate)
                .GreaterThanOrEqualTo(x => x.StartDate!.Value)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
        }
    }
    // Validator cho DTO UploadImageDto.
    public class UploadImageDtoValidator : AbstractValidator<UploadImageDto>
    {
        public UploadImageDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .Must(f => f != null && f.Length > 0)
                .WithMessage("File is required");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0);
        }
    }
    // Validator cho DTO ProductRegionPriceCreateDto.
    public class ProductRegionPriceCreateDtoValidator : AbstractValidator<ProductRegionPriceCreateDto>
    {
        public ProductRegionPriceCreateDtoValidator()
        {
            RuleFor(x => x.Region)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
    // Validator cho DTO ProductRegionPriceUpdateDto.
    public class ProductRegionPriceUpdateDtoValidator : AbstractValidator<ProductRegionPriceUpdateDto>
    {
        public ProductRegionPriceUpdateDtoValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0);
        }
    }
    // Validator cho DTO CreateWarrantyDto.
    public class CreateWarrantyDtoValidator : AbstractValidator<CreateWarrantyDto>
    {
        public CreateWarrantyDtoValidator()
        {
            RuleFor(x => x.SerialNumber)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.CustomerPhone)
                .NotEmpty()
                .Length(8, 20);

            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.WarrantyMonths)
                .InclusiveBetween(1, 120);
        }
    }
}



