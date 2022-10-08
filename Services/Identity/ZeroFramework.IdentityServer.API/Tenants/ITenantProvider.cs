using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Tenants
{
    public interface ITenantProvider
    {
        Task<IdentityTenant?> GetTenantAsync();

        Task<IdentityTenant?> FindTenantAsync(string tenantIdOrName);
    }
}
