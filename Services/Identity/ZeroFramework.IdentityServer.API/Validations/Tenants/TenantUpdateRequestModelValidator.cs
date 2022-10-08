using FluentValidation;
using ZeroFramework.IdentityServer.API.Models.Tenants;

namespace ZeroFramework.IdentityServer.API.Validations.Tenants
{
    public class TenantUpdateRequestModelValidator : AbstractValidator<TenantUpdateRequestModel>
    {
        public TenantUpdateRequestModelValidator(ILogger<TenantUpdateRequestModel> logger)
        {
            logger.LogInformation(nameof(TenantCreateRequestModelValidator));
            RuleFor(e => e.Id);
            RuleFor(e => e.Name).NotNull().NotEmpty().Length(5, 15).Matches("^[a-z]+$");
            RuleFor(e => e.DisplayName).NotNull().NotEmpty().Length(5, 15);
        }
    }
}