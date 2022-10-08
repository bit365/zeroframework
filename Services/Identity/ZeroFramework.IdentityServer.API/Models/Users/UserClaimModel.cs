using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Models.Users
{
    public class UserClaimModel
    {
        [AllowNull]
        public string ClaimType { get; set; }

        [AllowNull]
        public string ClaimValue { get; set; }


        public UserClaimModel(string claimType, string claimValue)
        {
            ClaimType = claimType;
            ClaimValue = claimValue;
        }
    }
}
