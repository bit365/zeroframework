using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate
{
    public class DeviceGrouping : BaseEntity, IMultiTenant
    {
        public long DeviceId { get; set; }

        [AllowNull]
        public Device Device { get; set; }

        public int DeviceGroupId { get; set; }

        [AllowNull]
        public DeviceGroup DeviceGroup { get; set; }

        public override object[] GetKeys() => new object[] { DeviceId, DeviceGroupId };

        public Guid? TenantId { get; set; }
    }
}
