using Luzanov.Application.Orders.Commands;
using FluentValidation;

namespace Luzanov.Application.Orders.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(o => o.CustomerFullName)
                .NotEmpty().WithMessage("ПІБ замовника є обов'язковим.")
                .MaximumLength(200).WithMessage("ПІБ не може перевищувати 200 символів.");

            RuleFor(o => o.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефону є обов'язковим.");

            RuleFor(o => o.DeliveryMethod)
                .NotEmpty().WithMessage("Спосіб доставки є обов'язковим.");

            RuleFor(o => o.PostService)
                .NotEmpty().When(o => o.DeliveryMethod == "Пошта")
                .WithMessage("Необхідно вказати поштовий сервіс при доставці поштою.");

            RuleFor(o => o.DeliveryAddress)
                .NotEmpty().WithMessage("Адреса доставки є обов'язковою.")
                .MaximumLength(500).WithMessage("Адреса не може перевищувати 500 символів.");

            RuleFor(o => o.PaymentMethod)
                .NotEmpty().WithMessage("Спосіб оплати є обов'язковим.")
                .MaximumLength(100).WithMessage("Спосіб оплати не може перевищувати 100 символів.");

            RuleFor(o => o.Comment)
                .MaximumLength(1000).WithMessage("Коментар не може перевищувати 1000 символів.");

            RuleFor(o => o.OrderItems)
                .NotNull().WithMessage("Список товарів є обов'язковим.")
                .Must(items => items != null && items.Count > 0)
                .WithMessage("Замовлення має містити хоча б один товар.");

            RuleForEach(o => o.OrderItems).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .GreaterThan(0).WithMessage("ID продукту має бути більше 0.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Кількість має бути більше 0.");

                item.RuleFor(i => i.Price)
                    .GreaterThanOrEqualTo(0).WithMessage("Ціна не може бути від'ємною.");
            });
        }
    }
}
