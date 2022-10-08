using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.Devices;

namespace ZeroFramework.DeviceCenter.Application.Validations.Devices
{
    public class DeviceCreateRequestModelValidator : AbstractValidator<DeviceCreateRequestModel>
    {
        public DeviceCreateRequestModelValidator()
        {
            RuleFor(m => m.Name).NotNull().NotEmpty().Length(5, 20);
            RuleFor(m => m.Remark).MaximumLength(100);
        }
    }
}