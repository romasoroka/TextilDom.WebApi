using FluentValidation;
using Luzanov.Application.SubCategories.Commands;

namespace Luzanov.Application.SubCategories.Validators
{
    public class CreateSubCategoryCommandValidator : AbstractValidator<CreateSubCategoryCommand>
    {
        public CreateSubCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва підкатегорії обов'язкова")
                .MaximumLength(200).WithMessage("Назва підкатегорії не може перевищувати 200 символів");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("ID категорії має бути більше 0");
        }
    }

    public class UpdateSubCategoryCommandValidator : AbstractValidator<UpdateSubCategoryCommand>
    {
        public UpdateSubCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID підкатегорії має бути більше 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва підкатегорії обов'язкова")
                .MaximumLength(200).WithMessage("Назва підкатегорії не може перевищувати 200 символів");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("ID категорії має бути більше 0");
        }
    }
}
