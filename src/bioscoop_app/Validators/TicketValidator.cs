using bioscoop_app.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace bioscoop_app.Validators
{
    class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(ticket => ticket.price).GreaterThanOrEqualTo(0);
            RuleFor(ticket => ticket.row).GreaterThanOrEqualTo(0);
            RuleFor(ticket => ticket.seatnr).GreaterThanOrEqualTo(0);
            RuleFor(ticket => ticket.screenTime).GreaterThan(0);
            RuleFor(ticket => ticket.visitorAge).GreaterThanOrEqualTo(0).LessThanOrEqualTo(120);
            RuleFor(ticket => ticket.type).Equal("ticket");
            RuleFor(ticket => ticket.name).NotNull().NotEmpty();
        }
    }
}
