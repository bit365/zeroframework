using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Users
{
    public class UserClaimModel(string claimType, string claimValue)
    {
        [AllowNull]
        public string ClaimType { get; set; } = claimType;

        [AllowNull]
        public string ClaimValue { get; set; } = claimValue;
    }
}
