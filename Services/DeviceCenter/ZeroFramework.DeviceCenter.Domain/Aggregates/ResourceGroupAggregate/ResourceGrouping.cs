using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ResourceGroupAggregate
{
    public class ResourceGrouping : BaseAggregateRoot<Guid>, IMultiTenant
    {
        [AllowNull]
        public ResourceDescriptor Resource { get; set; }

        public Guid ResourceGroupId { get; set; }

        public Guid? TenantId { get; set; }
    }
}