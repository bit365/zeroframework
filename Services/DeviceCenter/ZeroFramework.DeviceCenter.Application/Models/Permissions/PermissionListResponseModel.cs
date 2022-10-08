using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Permissions
{
    public class PermissionListResponseModel
    {
        [AllowNull]
        public string EntityDisplayName { get; set; }

        public List<PermissionGroupModel> Groups { get; set; } = new();
    }
}
