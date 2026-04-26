using FluentValidation;
using Textildom.Application.Products.Commands;

namespace Textildom.Application.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Id продукту має бути більшим за 0.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Назва продукту є обов'язковою.")
                .MaximumLength(200).WithMessage("Назва не може перевищувати 200 символів.");

            RuleFor(p => p.Description)
                .MaximumLength(5000).WithMessage("Опис не може перевищувати 5000 символів.");

            RuleFor(p => p.Variants)
                .NotNull().WithMessage("Список варіантів обов'язковий.")
                .Must(v => v != null && v.Count > 0).WithMessage("Необхідно вказати хоча б один варіант.");

            RuleForEach(p => p.Variants).ChildRules(variant =>
            {
                variant.RuleFor(v => v.Price)
                    .GreaterThan(0).WithMessage("Ціна має бути більше 0.");
                variant.RuleFor(v => v.OldPrice)
                    .GreaterThan(0).When(v => v.OldPrice.HasValue)
                    .WithMessage("Стара ціна має бути більше 0.");
            });

            RuleFor(p => p.CategoryId)
                .GreaterThan(0).When(p => p.CategoryId.HasValue)
                .WithMessage("Якщо вказана категорія, вона має бути більше 0.");
        }
    }
}