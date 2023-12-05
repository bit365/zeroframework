using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Permissions
{
    public class PermissionGroupModel
    {
        [AllowNull]
        public string Name { get; set; }

        [AllowNull]
        public string DisplayName { get; set; }

        public List<PermissionGrantModel> Permissions { get; set; } = [];
    }
}
