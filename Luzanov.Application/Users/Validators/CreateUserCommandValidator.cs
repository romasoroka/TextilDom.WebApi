using Luzanov.Application.Users.Commands;
using Luzanov.Domain.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Users.Validators
{
    /// <summary>
    /// Валідатор для команди створення користувача
    /// </summary>
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Ім'я користувача є обов'язковим")
                .MinimumLength(3).WithMessage("Ім'я користувача має містити мінімум 3 символи")
                .MaximumLength(50).WithMessage("Ім'я користувача не може перевищувати 50 символів");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим")
                .MinimumLength(6).WithMessage("Пароль має містити мінімум 6 символів")
                .Matches("[A-Z]").WithMessage("Пароль має містити хоча б одну велику літеру")
                .Matches("[a-z]").WithMessage("Пароль має містити хоча б одну малу літеру")
                .Matches("[0-9]").WithMessage("Пароль має містити хоча б одну цифру");

            // IsAdmin не потребує валідації - це булеве значення
        }
    }
}
