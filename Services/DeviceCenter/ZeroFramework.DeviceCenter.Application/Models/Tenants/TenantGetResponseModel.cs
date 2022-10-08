using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Tenants
{
    public class TenantGetResponseModel
    {
        public Guid Id { get; set; }

        [AllowNull]
        public string Name { get; set; }
    }
}