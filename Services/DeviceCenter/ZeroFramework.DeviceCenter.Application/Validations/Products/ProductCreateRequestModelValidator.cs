using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.Products;

namespace ZeroFramework.DeviceCenter.Application.Validations.Products
{
    public class ProductCreateRequestModelValidator : AbstractValidator<ProductCreateRequestModel>
    {
        public ProductCreateRequestModelValidator()
        {
            RuleFor(m => m.Name).NotNull().NotEmpty().Length(5, 20);
        }
    }
}
