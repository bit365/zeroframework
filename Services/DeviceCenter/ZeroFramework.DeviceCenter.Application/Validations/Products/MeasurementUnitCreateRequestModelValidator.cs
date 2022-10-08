using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.Products;

namespace ZeroFramework.DeviceCenter.Application.Validations.Products
{
    public class MeasurementUnitCreateRequestModelValidator : AbstractValidator<MeasurementUnitCreateRequestModel>
    {
        public MeasurementUnitCreateRequestModelValidator()
        {
            RuleFor(m => m.Unit).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(m => m.UnitName).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(m => m.Remark).MaximumLength(100);
        }
    }
}