using FluentValidation;
using ShopApi.DTOs.ProductImage;

namespace ShopApi.Validators
{
    // Validator cho DTO UploadImageDto.
    public class UploadImageDtoValidator : AbstractValidator<UploadImageDto>
    {
        public UploadImageDtoValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("File anh la bat buoc")
                .Must(f => f != null && f.Length > 0)
                .WithMessage("File anh khong hop le");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("SortOrder phai lon hon hoac bang 0");
        }
    }
}
