using FluentValidation;
using Textildom.Application.Orders.Commands;

namespace Textildom.Application.Orders.Validators
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

            RuleFor(o => o.CityRef)
                .NotEmpty().WithMessage("Місто є обов'язковим.");

            RuleFor(o => o.CityName)
                .NotEmpty().WithMessage("Назва міста є обов'язковою.");

            RuleFor(o => o.WarehouseRef)
                .NotEmpty().WithMessage("Відділення є обов'язковим.");

            RuleFor(o => o.WarehouseAddress)
                .NotEmpty().WithMessage("Адреса відділення є обов'язковою.");

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