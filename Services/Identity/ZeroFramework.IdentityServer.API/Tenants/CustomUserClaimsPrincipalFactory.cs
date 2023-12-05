using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Tenants
{
    public class CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> options, ApplicationDbContext dbContext) : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>(userManager, roleManager, options)
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity claimsIdentity = await base.GenerateClaimsAsync(user);

            if (user.TenantId.HasValue)
            {
                claimsIdentity.AddClaim(new Claim(TenantClaimTypes.TenantId, user.TenantId.Value.ToString()));
                IdentityTenant tenant = _dbContext.Set<IdentityTenant>().IgnoreQueryFilters().Single(t => t.Id == user.TenantId);
                claimsIdentity.AddClaim(new Claim(TenantClaimTypes.TenantName, tenant.Name));
            }

            return claimsIdentity;
        }
    }
}