using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Devices
{
    public class DeviceGroupUpdateRequestModel
    {
        /// <summary>
        /// 分组编号
        /// </summary>
        public int Id { get; set; }

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
        /// 父组编号
        /// </summary>
        public int? ParentId { get; set; }
    }
}
