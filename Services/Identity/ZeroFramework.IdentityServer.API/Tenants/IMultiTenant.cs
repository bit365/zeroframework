namespace ZeroFramework.IdentityServer.API.Tenants
{
    public interface IMultiTenant
    {
        Guid? TenantId { get; set; }
    }
}
