using Luzanov.Application.Products.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Products.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            Include(new CreateProductCommandValidator());

            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Id продукту має бути більшим за 0.");
        }
    }
}
