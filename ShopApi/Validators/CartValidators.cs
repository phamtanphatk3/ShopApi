using FluentValidation;
using ShopApi.DTOs.Cart;

namespace ShopApi.Validators
{
    // Validator cho DTO AddToCartDto.
    public class AddToCartDtoValidator : AbstractValidator<AddToCartDto>
    {
        public AddToCartDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0)
                .WithMessage("ProductId phai lon hon 0");

            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 9999)
                .WithMessage("So luong phai trong khoang tu 1 den 9999");
        }
    }

    // Validator cho DTO UpdateCartItemDto.
    public class UpdateCartItemDtoValidator : AbstractValidator<UpdateCartItemDto>
    {
        public UpdateCartItemDtoValidator()
        {
            RuleFor(x => x.Quantity)
                .InclusiveBetween(1, 9999)
                .WithMessage("So luong phai trong khoang tu 1 den 9999");
        }
    }
}
