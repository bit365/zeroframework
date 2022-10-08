namespace ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate
{
    public class TenantConnectionString
    {
        public virtual Guid TenantId { get; set; }

        public virtual string Name { get; set; } = string.Empty;

        public virtual string Value { get; set; } = string.Empty;
    }
}