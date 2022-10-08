using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Tenants
{
    public class TenantGetResponseModel
    {
        public Guid Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public string? DisplayName { get; set; }

        public string? ConnectionString { get; set; }

        public DateTimeOffset CreationTime { get; set; }
    }
}
