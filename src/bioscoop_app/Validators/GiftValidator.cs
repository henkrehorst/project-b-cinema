using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using bioscoop_app.Model;

namespace bioscoop_app.Validators
{
    class GiftValidator : AbstractValidator<Gift>
    {
        public GiftValidator()
        {
            RuleFor(gift => gift.Code).NotNull().NotEmpty();
        }
    }
}
