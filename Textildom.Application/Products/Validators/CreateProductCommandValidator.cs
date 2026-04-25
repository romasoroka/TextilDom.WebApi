using Textildom.Application.Products.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Products.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Назва продукту є обов'язковою.")
                .MaximumLength(200).WithMessage("Назва не може перевищувати 200 символів.");

            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Опис не може бути порожнім.")
                .MaximumLength(2000);

            RuleFor(p => p.Discount)
                .InclusiveBetween(0, 100).WithMessage("Знижка має бути від 0 до 100%.");

            RuleFor(p => p.Variants)
                .NotNull().WithMessage("Список варіантів обов'язковий.")
                .Must(variants => variants != null && variants.Count > 0)
                .WithMessage("Необхідно вказати хоча б один варіант товару.");

            RuleForEach(p => p.Variants).ChildRules(variant =>
            {
                variant.RuleFor(v => v.Width)
                    .GreaterThan(0).WithMessage("Ширина має бути більше 0.");
                variant.RuleFor(v => v.Height)
                    .GreaterThan(0).WithMessage("Висота має бути більше 0.");
                variant.RuleFor(v => v.Price)
                    .GreaterThan(0).WithMessage("Ціна має бути більше 0.");
                variant.RuleFor(v => v.Stock)
                    .GreaterThanOrEqualTo(0).WithMessage("Залишок не може бути від'ємним.");
            });

            RuleFor(p => p.CategoryId)
                .NotNull().WithMessage("Категорія є обов'язковою.")
                .GreaterThan(0).WithMessage("Необхідно вибрати категорію.");

            // SubCategoryId тепер необов'язкова
            RuleFor(p => p.SubCategoryId)
                .GreaterThan(0).When(p => p.SubCategoryId.HasValue)
                .WithMessage("Якщо вказана підкатегорія, вона має бути більше 0.");
        }
    }
}
