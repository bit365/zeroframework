using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.Devices;

namespace ZeroFramework.DeviceCenter.Application.Validations.Devices
{
    public class DeviceGroupCreateRequestModelValidator : AbstractValidator<DeviceGroupCreateRequestModel>
    {
        public DeviceGroupCreateRequestModelValidator()
        {
            RuleFor(m => m.Name).NotNull().NotEmpty().Length(5, 20);
            RuleFor(m => m.Remark).MaximumLength(100);
        }
    }
}