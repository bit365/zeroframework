using FluentValidation;
using Microsoft.Extensions.Logging;
using ZeroFramework.DeviceCenter.Application.Commands.Ordering;

namespace ZeroFramework.DeviceCenter.Application.Validations.Ordering
{
    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator(ILogger<CancelOrderCommandValidator> logger)
        {
            RuleFor(order => order.OrderId).NotEmpty().WithMessage("No orderId found");
            logger.LogInformation(nameof(CancelOrderCommandValidator));
        }
    }
}