using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.ResourceGroups
{
    public class ResourceGroupUpdateRequestModel
    {
        public Guid Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public string? DisplayName { get; set; }
    }
}
