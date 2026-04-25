using FluentValidation;
using Textildom.Application.Categories.Commands;

namespace Textildom.Application.Categories.Validators
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва категорії обов'язкова")
                .MaximumLength(200).WithMessage("Назва категорії не може перевищувати 200 символів");
        }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("ID категорії має бути більше 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва категорії обов'язкова")
                .MaximumLength(200).WithMessage("Назва категорії не може перевищувати 200 символів");
        }
    }
}
