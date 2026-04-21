using FluentValidation;
using ShopApi.DTOs.Auth;

namespace ShopApi.Validators
{
    // Validator cho DTO LoginRequest.
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username la bat buoc")
                .Length(3, 50)
                .WithMessage("Username phai tu 3 den 50 ky tu");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mat khau la bat buoc")
                .Length(3, 100)
                .WithMessage("Mat khau phai tu 3 den 100 ky tu");
        }
    }
}
