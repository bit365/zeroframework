using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Permissions
{
    public class PermissionGrantInfoModel
    {
        [AllowNull]
        public string Name { get; set; }

        public bool IsGranted { get; set; }
    }
}
