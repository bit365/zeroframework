using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ZeroFramework.EventBus.Extensions;

namespace ZeroFramework.DeviceCenter.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidatorBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger = logger;

        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var typeName = request.GetGenericTypeName();

            _logger.LogInformation("Validating command {CommandType}", typeName);

            var failures = _validators.Select(v => v.Validate(request)).SelectMany(result => result.Errors).Where(error => error != null).ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);
                throw new System.ApplicationException($"Command Validation Errors for type {typeof(TRequest).Name}", new ValidationException("Validation exception", failures));
            }

            return await next();
        }
    }
}