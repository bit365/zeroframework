namespace ZeroFramework.IdentityServer.API.Tenants
{
    public interface ICurrentTenantAccessor
    {
        TenantInfo? Current { get; set; }
    }
}