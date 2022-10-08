using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate
{
    public class PermissionGrant : BaseAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        [AllowNull]
        public string OperationName { get; set; }

        [AllowNull]
        public string ProviderName { get; set; }

        [AllowNull]
        public string ProviderKey { get; set; }

        public Guid? ResourceGroupId { get; set; }
    }
}