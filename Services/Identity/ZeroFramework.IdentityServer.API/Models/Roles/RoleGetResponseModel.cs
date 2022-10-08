using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Roles
{
    public class RoleGetResponseModel
    {
        public int Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        [AllowNull]
        public string TenantRoleName { get; set; }

        public string? DisplayName { get; set; }

        public DateTimeOffset CreationTime { get; set; }
    }
}