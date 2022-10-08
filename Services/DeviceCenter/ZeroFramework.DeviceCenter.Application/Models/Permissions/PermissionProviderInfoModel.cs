using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Permissions
{
    public class PermissionProviderInfoModel
    {
        [AllowNull]
        public string ProviderName { get; set; }

        [AllowNull]
        public string ProviderKey { get; set; }
    }
}
