using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;

namespace ZeroFramework.DeviceCenter.Application.Models.Products
{
    public class ProductUpdateRequestModel
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [AllowNull]
        public string Name { get; set; }

        /// <summary>
        /// 物模型描述
        /// </summary>
        public ProductFeatures? Features { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string? Remark { get; set; }
    }
}
