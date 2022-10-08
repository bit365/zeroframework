using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Tenants
{
    public class TenantMiddleware : IMiddleware
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly ITenantProvider _tenantProvider;

        public TenantMiddleware(ICurrentTenant currentTenant, ITenantProvider tenantProvider)
        {
            _currentTenant = currentTenant;
            _tenantProvider = tenantProvider;
        }

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
