using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Users
{
    public class UserGetResponseModel
    {
        public int Id { get; set; }

        [AllowNull]
        public string UserName { get; set; }

        [AllowNull]
        public string TenantUserName { get; set; }

        [AllowNull]
        public string PhoneNumber { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public string? DisplayName { get; set; }

        public DateTimeOffset CreationTime { get; set; }
    }
}