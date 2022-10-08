using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.ResourceGroups
{
    public class ResourceGroupGetResponseModel
    {
        public Guid Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public Guid? TenantId { get; set; }
    }
}
