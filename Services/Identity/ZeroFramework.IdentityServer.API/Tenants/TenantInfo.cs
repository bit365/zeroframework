namespace ZeroFramework.IdentityServer.API.Tenants
{
    public class TenantInfo
    {
        /// <summary>
        /// Null indicates the host.
        /// Not null value for a tenant.
        /// </summary>
        public Guid? TenantId { get; }

        /// <summary>
        /// Name of the tenant if <see cref="TenantId"/> is not null.
        /// </summary>
        public string? Name { get; }

        public TenantInfo(Guid? tenantId, string? name = null)
        {
            TenantId = tenantId;
            Name = name;
        }
    }
}
