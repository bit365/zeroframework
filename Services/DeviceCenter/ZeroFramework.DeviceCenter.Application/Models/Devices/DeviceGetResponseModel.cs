using System.Diagnostics.CodeAnalysis;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Application.Models.Devices
{
    public class DeviceGetResponseModel
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [AllowNull]
        public string Name { get; set; }

        /// <summary>
        /// 设备状态
        /// </summary>
        [AllowNull]
        public DeviceStatus Status { get; set; }

        /// <summary>
        /// 所属产品
        /// </summary>
        [AllowNull]
        public int ProductId { get; set; }

        /// <summary>
        /// 所属产品
        /// </summary>
        [AllowNull]
        public ProductGetResponseModel Product { get; set; }

        /// <summary>
        /// 经纬度坐标
        /// </summary>
        [AllowNull]
        public string? Coordinate { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 最后在线时间
        /// </summary>
        public DateTimeOffset? LastOnlineTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }
    }
}
