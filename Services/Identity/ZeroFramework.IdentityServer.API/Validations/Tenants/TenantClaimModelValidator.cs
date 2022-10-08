using FluentValidation;
using ZeroFramework.IdentityServer.API.Models.Tenants;

namespace ZeroFramework.IdentityServer.API.Validations.Tenants
{
    public class TenantClaimModelValidator : AbstractValidator<TenantClaimModel>
    {
        public TenantClaimModelValidator()
        {
            RuleFor(e => e.ClaimType).NotNull().NotEmpty().Length(5, 15);
            RuleFor(e => e.ClaimValue).Length(5, 15);
        }
    }
}