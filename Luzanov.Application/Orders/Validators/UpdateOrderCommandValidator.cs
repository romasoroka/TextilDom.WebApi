using Luzanov.Application.Orders.Commands;
using FluentValidation;

namespace Luzanov.Application.Orders.Validators
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

            RuleFor(o => o.DeliveryMethod)
                .NotEmpty().WithMessage("Спосіб доставки є обов'язковим.");

            RuleFor(o => o.DeliveryAddress)
                .NotEmpty().WithMessage("Адреса доставки є обов'язковою.")
                .MaximumLength(500).WithMessage("Адреса не може перевищувати 500 символів.");

            RuleFor(o => o.PaymentMethod)
                .NotEmpty().WithMessage("Спосіб оплати є обов'язковим.")
                .MaximumLength(100).WithMessage("Спосіб оплати не може перевищувати 100 символів.");

            RuleFor(o => o.OrderStatus)
                .NotEmpty().WithMessage("Статус замовлення є обов'язковим.");
        }
    }
}
