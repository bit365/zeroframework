using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Tenants
{
    public class TenantMiddleware(ICurrentTenant currentTenant, ITenantProvider tenantProvider) : IMiddleware
    {
        private readonly ICurrentTenant _currentTenant = currentTenant;
        private readonly ITenantProvider _tenantProvider = tenantProvider;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            IdentityTenant? currentTenant = await _tenantProvider.GetTenantAsync();

            using (_currentTenant.Change(currentTenant?.Id, currentTenant?.Name))
            {
                await next(context);
            }
        }
    }
}
