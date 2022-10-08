using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Tenants
{
    public class TenantUpdateRequestModel
    {
        public Guid Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? ConnectionString { get; set; }
    }
}
