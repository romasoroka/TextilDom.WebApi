using Textildom.Application.Login.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Textildom.Application.Login.Validators
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Ім’я користувача є обов’язковим")
                .MinimumLength(3).WithMessage("Ім’я користувача має містити мінімум 3 символи");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов’язковим")
                .MinimumLength(6).WithMessage("Пароль має містити мінімум 6 символів");
        }
    }
}
