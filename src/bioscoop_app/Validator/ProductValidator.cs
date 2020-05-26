using bioscoop_app.Model;
using FluentValidation;

namespace bioscoop_app.Validator
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.name).NotEmpty().WithMessage("Product naam ontbreekt!");
            RuleFor(x => x.price).NotEmpty().WithMessage("Prijs ontbreekt");
            RuleFor(x => x.type).Must(ValidateProductType).WithMessage("Verkeerde producttype!");
        }

        private bool ValidateProductType(string productType)
        {
            return productType == "upsell" || productType == "ticket" || productType == "product";
        }
    }
}