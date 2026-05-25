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

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
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

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .WithMessage("Email khong duoc vuot qua 150 ky tu")
                .EmailAddress()
                .WithMessage("Email khong hop le")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Phone)
                .MaximumLength(20)
                .WithMessage("Phone khong duoc vuot qua 20 ky tu")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone));

            RuleFor(x => x.Address)
                .MaximumLength(255)
                .WithMessage("Address khong duoc vuot qua 255 ky tu")
                .When(x => !string.IsNullOrWhiteSpace(x.Address));
        }
    }

    public class ChangePasswordRequestDtoValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordRequestDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Mat khau hien tai la bat buoc")
                .Length(3, 100)
                .WithMessage("Mat khau hien tai phai tu 3 den 100 ky tu");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Mat khau moi la bat buoc")
                .Length(3, 100)
                .WithMessage("Mat khau moi phai tu 3 den 100 ky tu");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("ConfirmPassword phai khop voi NewPassword");
        }
    }
}
