using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.Extensions
{
    public class PermissionRequirementHandler(ICurrentTenant currentTenant) : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly ICurrentTenant _currentTenant = currentTenant;

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement)
        {
            string tenantOwnerRequireRole = AuthorizeConstants.TenantOwnerRequireRole;

            if (_currentTenant.Name is not null)
            {
                tenantOwnerRequireRole += $"@{_currentTenant.Name}";
            }

            if (context.User.IsInRole(tenantOwnerRequireRole))
            {
                context.Succeed(requirement);
            }

            await Task.CompletedTask;
        }
    }
}