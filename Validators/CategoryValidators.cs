using FluentValidation;
using ShopApi.DTOs.Category;

namespace ShopApi.Validators
{
    // Validator cho DTO CategoryCreateDto.
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Ten danh muc la bat buoc")
                .Length(2, 100)
                .WithMessage("Ten danh muc phai tu 2 den 100 ky tu");

            RuleFor(x => x.Slug)
                .MaximumLength(120)
                .WithMessage("Slug khong duoc vuot qua 120 ky tu");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(0)
                .WithMessage("ParentCategoryId phai lon hon 0")
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
                .WithMessage("Ten danh muc la bat buoc")
                .Length(2, 100)
                .WithMessage("Ten danh muc phai tu 2 den 100 ky tu");

            RuleFor(x => x.Slug)
                .MaximumLength(120)
                .WithMessage("Slug khong duoc vuot qua 120 ky tu");
        }
    }
}
