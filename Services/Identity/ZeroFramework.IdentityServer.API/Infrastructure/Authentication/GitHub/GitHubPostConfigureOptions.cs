/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using Microsoft.Extensions.Options;
using static ZeroFramework.IdentityServer.API.Infrastructure.Authentication.GitHub.GitHubAuthenticationDefaults;

namespace ZeroFramework.IdentityServer.API.Infrastructure.Authentication.GitHub;

/// <summary>
/// A class used to setup defaults for all <see cref="GitHubAuthenticationOptions"/>.
/// </summary>
public class GitHubPostConfigureOptions : IPostConfigureOptions<GitHubAuthenticationOptions>
{
    public void PostConfigure(string? name, GitHubAuthenticationOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.EnterpriseDomain))
        {
            options.AuthorizationEndpoint = CreateUrl(options.EnterpriseDomain, AuthorizationEndpointPath);
            options.TokenEndpoint = CreateUrl(options.EnterpriseDomain, TokenEndpointPath);
            options.UserEmailsEndpoint = CreateUrl(options.EnterpriseDomain, EnterpriseApiPath + UserEmailsEndpointPath);
            options.UserInformationEndpoint = CreateUrl(options.EnterpriseDomain, EnterpriseApiPath + UserInformationEndpointPath);
        }
    }

    private static string CreateUrl(string domain, string path)
    {
        // Enforce use of HTTPS
        var builder = new UriBuilder(domain)
        {
            Path = path,
            Port = -1,
            Scheme = Uri.UriSchemeHttps,
        };

        return builder.Uri.ToString();
    }
}
