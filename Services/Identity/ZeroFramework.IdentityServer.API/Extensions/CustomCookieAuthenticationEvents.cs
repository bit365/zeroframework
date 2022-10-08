using Microsoft.AspNetCore.Authentication.Cookies;

namespace ZeroFramework.IdentityServer.API.Extensions
{
    /// <summary>
    /// Once a cookie is created, the cookie is the single source of identity. If a user account is disabled in back-end systems:
    /// The app's cookie authentication system continues to process requests based on the authentication cookie.
    /// The user remains signed into the app as long as the authentication cookie is valid.
    /// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie#react-to-back-end-changes
    /// </summary>
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            return base.ValidatePrincipal(context);
        }
    }
}
