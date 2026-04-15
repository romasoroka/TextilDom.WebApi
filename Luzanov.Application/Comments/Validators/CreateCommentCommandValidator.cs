using FluentValidation;
using Luzanov.Application.Comments.Commands;

namespace Luzanov.Application.Comments.Validators
{
    /// <summary>
    /// Валідатор для команди створення коментаря
    /// </summary>
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            // Валідація імені коментатора
            RuleFor(x => x.CommenterName)
                .NotEmpty().WithMessage("Ім'я коментатора є обов'язковим")
                .MaximumLength(100).WithMessage("Ім'я коментатора не може перевищувати 100 символів");

            // Валідація оцінки
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Оцінка має бути від 1 до 5");

            // Валідація тексту коментаря
            RuleFor(x => x.CommentText)
                .NotEmpty().WithMessage("Текст коментаря є обов'язковим")
                .MaximumLength(500).WithMessage("Коментар не може перевищувати 500 символів");

            // Валідація ProductId
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ID товару має бути більше 0");
        }
    }
}
