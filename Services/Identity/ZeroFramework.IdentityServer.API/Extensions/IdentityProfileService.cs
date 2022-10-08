using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Extensions
{
    /// <summary>
    /// Often IdentityServer requires identity information about users when creating tokens or when handling 
    /// requests to the userinfo or introspection endpoints. By default, IdentityServer only has the claims 
    /// in the authentication cookie to draw upon for this identity data.
    /// https://Duende.IdentityServer.readthedocs.io/en/latest/reference/profileservice.html
    /// It is impractical to put all of the possible claims needed for users into the cookie, so IdentityServer
    /// defines an extensibility point for allowing claims to be dynamically loaded as needed for a user. 
    /// This extensibility point is the IProfileService and it is common for a developer to implement this 
    /// interface to access a custom database or API that contains the identity data for users.
    /// </summary>
    public class IdentityProfileService : ProfileService<ApplicationUser>
    {
        public IdentityProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory) : base(userManager, claimsFactory)
        {
        }

        public IdentityProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<ProfileService<ApplicationUser>> logger) : base(userManager, claimsFactory, logger)
        {
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            return base.GetProfileDataAsync(context);
        }
    }
}
