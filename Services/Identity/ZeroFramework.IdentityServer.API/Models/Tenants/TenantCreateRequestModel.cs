using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Tenants
{
    public class TenantCreateRequestModel
    {
        [AllowNull]
        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? AdminUserName { get; set; }

        public string? AdminPassword { get; set; }

        public string? ConnectionString { get; set; }
    }
}
