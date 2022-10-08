using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate
{
    public class Tenant : BaseAggregateRoot<Guid>
    {
        [AllowNull]
        public string Name { get; set; }

        public List<TenantConnectionString> ConnectionStrings { get; protected set; } = new List<TenantConnectionString>();
    }
}