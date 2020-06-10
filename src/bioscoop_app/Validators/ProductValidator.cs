using bioscoop_app.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace bioscoop_app.Validators
{
    class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(product => product.name).NotNull().NotEmpty();
        }
    }
}
