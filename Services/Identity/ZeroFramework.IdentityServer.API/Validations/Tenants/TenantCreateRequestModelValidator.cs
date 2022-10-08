using FluentValidation;
using ZeroFramework.IdentityServer.API.Models.Tenants;

namespace ZeroFramework.IdentityServer.API.Validations.Tenants
{
    public class TenantCreateRequestModelValidator : AbstractValidator<TenantCreateRequestModel>
    {
        public TenantCreateRequestModelValidator(ILogger<TenantCreateRequestModelValidator> logger)
        {
            logger.LogInformation(nameof(TenantCreateRequestModelValidator));
            RuleFor(e => e.Name).NotNull().NotEmpty().Length(5, 15).Matches("^[a-z]+$");
            RuleFor(e => e.AdminUserName).NotNull().NotEmpty().Length(5, 15);
            RuleFor(e => e.AdminPassword).NotNull().NotEmpty().Length(5, 15);
            RuleFor(e => e.DisplayName).NotNull().NotEmpty().Length(5, 15);
        }
    }
}