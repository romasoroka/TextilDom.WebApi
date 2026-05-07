using FluentValidation;
using Textildom.Application.Orders.Commands;

namespace Textildom.Application.Orders.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(o => o.Id)
                .GreaterThan(0).WithMessage("Id замовлення має бути більшим за 0.");

            RuleFor(o => o.CustomerFullName)
                .NotEmpty().WithMessage("ПІБ замовника є обов'язковим.")
                .MaximumLength(200).WithMessage("ПІБ не може перевищувати 200 символів.");

            RuleFor(o => o.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефону є обов'язковим.");

            RuleFor(o => o.CityRef)
                .NotEmpty().WithMessage("Місто є обов'язковим.");

            RuleFor(o => o.WarehouseRef)
                .NotEmpty().WithMessage("Відділення є обов'язковим.");

            RuleFor(o => o.OrderStatus)
                .NotEmpty().WithMessage("Статус замовлення є обов'язковим.");

            RuleFor(o => o.Comment)
                .MaximumLength(1000).WithMessage("Коментар не може перевищувати 1000 символів.");
        }
    }
}