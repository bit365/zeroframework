using FluentValidation;
using Microsoft.Extensions.Logging;
using ZeroFramework.DeviceCenter.Application.Commands.Products;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.Models.Products;

namespace ZeroFramework.DeviceCenter.Application.Validations.Products
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(m => m.Name).NotNull().NotEmpty().Length(5, 20);
        }
    }

    public class CreateProductIdentifiedCommandValidator : AbstractValidator<IdentifiedCommand<CreateProductCommand, ProductGetResponseModel>>
    {
        public CreateProductIdentifiedCommandValidator(ILogger<CreateProductIdentifiedCommandValidator> logger)
        {
            logger.LogInformation(GetType().Name);
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}