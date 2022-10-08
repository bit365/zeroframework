using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Devices
{
    public class DeviceGroupCreateRequestModel
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
        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 父组编号
        /// </summary>
        public int? ParentId { get; set; }
    }
}
