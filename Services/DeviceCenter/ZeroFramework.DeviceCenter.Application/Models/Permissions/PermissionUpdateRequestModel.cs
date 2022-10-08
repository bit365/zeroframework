using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Permissions
{
    public class PermissionUpdateRequestModel
    {
        [AllowNull]
        public IEnumerable<PermissionProviderInfoModel> ProviderInfos { get; set; }

        [AllowNull]
        public IEnumerable<PermissionGrantInfoModel> PermissionGrantInfos { get; set; }

        public Guid? ResourceGroupId { get; set; }
    }
}