using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.ResourceGroups;

namespace ZeroFramework.DeviceCenter.Application.Validations.ResourceGroups
{
    public class ResourceGroupCreateRequestModelValidator : AbstractValidator<ResourceGroupCreateRequestModel>
    {
        public ResourceGroupCreateRequestModelValidator()
        {
            RuleFor(m => m.Name).NotNull().NotEmpty().Length(5, 20);
            RuleFor(m => m.DisplayName).Length(5, 20);
        }
    }
}
