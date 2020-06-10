using bioscoop_app.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bioscoop_app.Validators
{
    class MovieValidator : AbstractValidator<Movie>
    {
        public MovieValidator(bool requireImages = true)
        {
            if (requireImages) {
                RuleFor(movie => movie.coverImage).NotNull().NotEmpty();
                RuleFor(movie => movie.thumbnailImage).NotNull().NotEmpty();
            }
            RuleFor(movie => movie.duration).GreaterThan(0);
            RuleFor(movie => movie.genre).NotNull().NotEmpty();
            RuleFor(movie => movie.kijkwijzer).NotNull().Must(list => list.Length > 0);
            RuleFor(movie => movie.rating).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
            RuleFor(movie => movie.title).NotNull().NotEmpty();
            RuleFor(movie => movie.samenvatting).NotNull().NotEmpty().Length(20, 2000);
        }
    }
}
