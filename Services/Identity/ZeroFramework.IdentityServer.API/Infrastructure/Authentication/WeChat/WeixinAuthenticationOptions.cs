/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using static ZeroFramework.IdentityServer.API.Infrastructure.Authentication.Weixin.WeixinAuthenticationConstants;

namespace ZeroFramework.IdentityServer.API.Infrastructure.Authentication.Weixin;

/// <summary>
/// Defines a set of options used by <see cref="WeixinAuthenticationHandler"/>.
/// </summary>
public class WeixinAuthenticationOptions : OAuthOptions
{
    public WeixinAuthenticationOptions()
    {
        ClaimsIssuer = WeixinAuthenticationDefaults.Issuer;
        CallbackPath = WeixinAuthenticationDefaults.CallbackPath;

        AuthorizationEndpoint = WeixinAuthenticationDefaults.AuthorizationEndpoint;
        TokenEndpoint = WeixinAuthenticationDefaults.TokenEndpoint;
        UserInformationEndpoint = WeixinAuthenticationDefaults.UserInformationEndpoint;

        Scope.Add("snsapi_login");
        Scope.Add("snsapi_userinfo");

        ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
        ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
        ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
        ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
        ClaimActions.MapJsonKey(Claims.OpenId, "openid");
        ClaimActions.MapJsonKey(Claims.Province, "province");
        ClaimActions.MapJsonKey(Claims.City, "city");
        ClaimActions.MapJsonKey(Claims.HeadImgUrl, "headimgurl");
        ClaimActions.MapCustomJson(Claims.Privilege, user =>
        {
            if (!user.TryGetProperty("privilege", out var value) || value.ValueKind != System.Text.Json.JsonValueKind.Array)
            {
                return null;
            }

            return string.Join(',', value.EnumerateArray().Select(element => element.GetString()));
        });
    }
}
