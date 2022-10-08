using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Tenants
{
    public class TenantCreateOrUpdateRequestModel
    {
        public Guid Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public DateTimeOffset CreationTime { get; set; }
    }
}
