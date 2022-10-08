using FluentValidation;
using ZeroFramework.DeviceCenter.Application.Models.Monitoring;

namespace ZeroFramework.DeviceCenter.Application.Validations.Monitoring
{
    public class MonitoringFactorCreateRequestModelValidator : AbstractValidator<MonitoringFactorCreateRequestModel>
    {
        public MonitoringFactorCreateRequestModelValidator()
        {
            RuleFor(m => m.FactorCode).NotNull().NotEmpty().Length(6);
            RuleFor(m => m.ChineseName).Length(2, 15);
            RuleFor(m => m.EnglishName).Length(2, 15);
            RuleFor(m => m.Unit).Length(0, 10);
            RuleFor(m => m.Remarks).Length(2, 100);
        }
    }
}