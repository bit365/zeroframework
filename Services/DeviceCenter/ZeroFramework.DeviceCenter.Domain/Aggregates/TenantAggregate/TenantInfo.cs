namespace ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate
{
    public class TenantInfo(Guid? tenantId, string? name = null)
    {
        /// <summary>
        /// Null indicates the host.
        /// Not null value for a tenant.
        /// </summary>
        public Guid? TenantId { get; } = tenantId;

        /// <summary>
        /// Name of the tenant if <see cref="TenantId"/> is not null.
        /// </summary>
        public string? Name { get; } = name;
    }
}
