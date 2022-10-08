using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Application.Models.Products
{
    public class ProductGetResponseModel
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public Guid Id { get; set; }

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
    }
}
