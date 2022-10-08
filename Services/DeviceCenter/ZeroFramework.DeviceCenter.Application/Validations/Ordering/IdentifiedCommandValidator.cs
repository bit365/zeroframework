using FluentValidation;
using Microsoft.Extensions.Logging;
using ZeroFramework.DeviceCenter.Application.Commands.Ordering;
using ZeroFramework.DeviceCenter.Application.Infrastructure;

namespace ZeroFramework.DeviceCenter.Application.Validations.Ordering
{
    public class IdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CancelOrderCommand, bool>>
    {
        public IdentifiedCommandValidator(ILogger<IdentifiedCommandValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty();
            logger.LogInformation(GetType().Name);
        }
    }
}