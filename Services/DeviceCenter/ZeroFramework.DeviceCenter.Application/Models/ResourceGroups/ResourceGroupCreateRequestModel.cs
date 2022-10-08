using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.ResourceGroups
{
    public class ResourceGroupCreateRequestModel
    {
        [AllowNull]
        public string Name { get; set; }

        public string? DisplayName { get; set; }
    }
}
