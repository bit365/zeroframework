using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.Monitoring;

namespace ZeroFramework.DeviceCenter.Application.Validations.Monitoring
{
    public class MonitoringFactorUpdateRequestModelValidator : AbstractValidator<MonitoringFactorUpdateRequestModel>
    {
        public MonitoringFactorUpdateRequestModelValidator()
        {
            RuleFor(m => m.FactorCode).NotNull().NotEmpty().Length(6);
            RuleFor(m => m.ChineseName).Length(5, 20);
            RuleFor(m => m.EnglishName).Length(5, 36);
            RuleFor(m => m.Remarks).Length(5, 100);
        }
    }
}
