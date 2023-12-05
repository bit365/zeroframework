using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Permissions
{
    public class PermissionGrantModel
    {
        [AllowNull]
        public string Name { get; set; }

        [AllowNull]
        public string DisplayName { get; set; }

        [AllowNull]
        public string ParentName { get; set; }

        public bool IsGranted { get; set; }

        public List<string> AllowedProviders { get; set; } = [];
    }
}
