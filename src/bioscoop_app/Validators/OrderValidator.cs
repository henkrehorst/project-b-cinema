using System;
using System.Collections.Generic;
using System.Text;
using bioscoop_app.Model;
using FluentValidation;

namespace bioscoop_app.Validators
{
    class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.cust_name).NotNull().NotEmpty();
            RuleFor(order => order.cust_email).EmailAddress();
        }
    }
}
