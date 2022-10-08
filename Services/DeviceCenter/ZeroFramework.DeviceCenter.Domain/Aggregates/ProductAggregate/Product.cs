using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Entities;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class Product : BaseAggregateRoot<Guid>, ISoftDelete, IMultiTenant
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [AllowNull]
        public string Name { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        [AllowNull]
        public ProductNodeType NodeType { get; set; }

        /// <summary>
        /// 网络类型
        /// </summary>
        [AllowNull]
        public ProductNetType NetType { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        [AllowNull]
        public ProductProtocolType ProtocolType { get; set; }

        /// <summary>
        /// 数据格式
        /// </summary>
        [AllowNull]
        public ProductDataFormat DataFormat { get; set; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 物模型描述
        /// </summary>
        public ProductFeatures? Features { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// 租户标识
        /// </summary>
        public Guid? TenantId { get; set; }
    }
}
