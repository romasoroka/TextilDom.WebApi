using Luzanov.Application.Products.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Products.Validators
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
                variant.RuleFor(v => v.Size)
                    .NotEmpty().WithMessage("Розмір не може бути порожнім.");
                
                variant.RuleFor(v => v.Price)
                    .GreaterThan(0).WithMessage("Основна ціна має бути більше 0.");

                // Валідація оптових цін (якщо вказані)
                variant.RuleFor(v => v.PriceFrom10)
                    .GreaterThan(0).When(v => v.PriceFrom10.HasValue)
                    .WithMessage("Ціна від 10 шт має бути більше 0.")
                    .LessThan(v => v.Price).When(v => v.PriceFrom10.HasValue)
                    .WithMessage("Ціна від 10 шт має бути менше основної ціни.");

                variant.RuleFor(v => v.PriceFrom20)
                    .GreaterThan(0).When(v => v.PriceFrom20.HasValue)
                    .WithMessage("Ціна від 20 шт має бути більше 0.")
                    .LessThan(v => v.Price).When(v => v.PriceFrom20.HasValue)
                    .WithMessage("Ціна від 20 шт має бути менше основної ціни.")
                    .LessThanOrEqualTo(v => v.PriceFrom10 ?? decimal.MaxValue).When(v => v.PriceFrom20.HasValue && v.PriceFrom10.HasValue)
                    .WithMessage("Ціна від 20 шт має бути менше або дорівнювати ціні від 10 шт.");

                variant.RuleFor(v => v.PriceFrom50)
                    .GreaterThan(0).When(v => v.PriceFrom50.HasValue)
                    .WithMessage("Ціна від 50 шт має бути більше 0.")
                    .LessThan(v => v.Price).When(v => v.PriceFrom50.HasValue)
                    .WithMessage("Ціна від 50 шт має бути менше основної ціни.")
                    .LessThanOrEqualTo(v => v.PriceFrom20 ?? decimal.MaxValue).When(v => v.PriceFrom50.HasValue && v.PriceFrom20.HasValue)
                    .WithMessage("Ціна від 50 шт має бути менше або дорівнювати ціні від 20 шт.");
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
