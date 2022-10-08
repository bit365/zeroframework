using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate
{
    public class DeviceGroup : BaseAggregateRoot<int>, IMultiTenant
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        [AllowNull]
        public string Name { get; set; }

        /// <summary>
        /// 备注描述
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// 父组编号
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// 父组实体
        /// </summary>
        public DeviceGroup? Parent { get; set; }

        /// <summary>
        /// 子组列表
        /// </summary>
        public List<DeviceGroup>? Children { get; set; }

        /// <summary>
        /// 设备列表
        /// </summary>
        public List<Device>? Devices { get; set; }

        /// <summary>
        /// 租户编号
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
