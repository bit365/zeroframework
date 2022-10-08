using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Devices
{
    public class DeviceUpdateRequestModel
    {
        /// <summary>
        /// 产品编号
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        [AllowNull]
        public string Name { get; set; }

        /// <summary>
        /// 经纬度坐标
        /// </summary>
        [AllowNull]
        public string? Coordinate { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string? Remark { get; set; }
    }
}
