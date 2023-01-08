using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    public static class SampleDatas
    {
        public static IEnumerable<IdentityResource> Ids => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("userinfo", "Your user information", new []
            {
                JwtClaimTypes.Role,
                JwtRegisteredClaimNames.UniqueName,
                JwtClaimTypes.NickName,
                JwtClaimTypes.Address,
                JwtClaimTypes.Email
            })
        };

        public static IEnumerable<ApiResource> Apis => new List<ApiResource>
        {
            new ApiResource("devicecenterapi", "Device Center API",new []{ JwtClaimTypes.Email })
            {
                Scopes= { "openapi", "devicecenter" }
            },
            new ApiResource("identityserverapi", "Identity Server API",new []{ JwtClaimTypes.PhoneNumber,JwtClaimTypes.BirthDate})
            {
                Scopes= { "openapi", "identityserver" }
            }
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("openapi", "All open web api", new []{ JwtClaimTypes.Role, TenantClaimTypes.TenantId, TenantClaimTypes.TenantName, JwtClaimTypes.Name}),
            new ApiScope("identityserver", "Identity server api", new []{ JwtClaimTypes.Role, TenantClaimTypes.TenantId, TenantClaimTypes.TenantName, JwtClaimTypes.Name}),
            new ApiScope("devicecenter", "Device center api", new []{ JwtClaimTypes.Role, TenantClaimTypes.TenantId, TenantClaimTypes.TenantName, JwtClaimTypes.Name})
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "devicecenterswagger",
                ClientName = "Device Center Swagger",
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret=false,
                AlwaysSendClientClaims=true,
                AlwaysIncludeUserClaimsInIdToken=true,
                RequireConsent = false,
                RedirectUris = {
                    "https://localhost:6001/swagger/oauth2-redirect.html",
                    "https://devicecenterapi.helloworldnet.com/oauth2-redirect.html"
                },
                PostLogoutRedirectUris = {
                    "https://localhost:6001/swagger",
                    "https://devicecenterapi.helloworldnet.com/swagger"
                },
                AllowOfflineAccess=true,
                RequirePkce = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "devicecenter"
                },
                AllowedCorsOrigins = {
                    "https://localhost:6001",
                    "https://devicecenterapi.helloworldnet.com"
                },
                AccessTokenLifetime = 600
            },
            new Client
            {
                ClientId = "identityserverswagger",
                ClientName = "Identity Server Swagger",
                ClientSecrets = { new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret=false,
                AlwaysSendClientClaims=true,
                AlwaysIncludeUserClaimsInIdToken=true,
                RequireConsent = false,
                RedirectUris = {
                    "https://localhost:5001/swagger/oauth2-redirect.html",
                    "https://identityserver.helloworldnet.com"
                },
                PostLogoutRedirectUris = {
                    "https://localhost:5001/swagger",
                    "https://identityserver.helloworldnet.com/swagger"
                },
                AllowOfflineAccess=true,
                RequirePkce = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "identityserver"
                },
                AllowedCorsOrigins = {
                    "https://localhost:5001",
                    "https://identityserver.helloworldnet.com"
                },
                AccessTokenLifetime = 600
            },
            new Client
            {
                ClientId = "devicecenterweb",
                ClientName = "Device Center Web",
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = true,
                RequireClientSecret=false,
                AlwaysSendClientClaims=true,
                AlwaysIncludeUserClaimsInIdToken=true,
                RequireConsent = false,
                RedirectUris = {
                    "http://localhost:8000/authorization/login-callback",
                    "https://cloud.helloworldnet.com/authorization/login-callback"
                },
                PostLogoutRedirectUris = {
                    "http://localhost:8000/authorization/logout-callback",
                    "https://cloud.helloworldnet.com/authorization/logout-callback"
                },
                AllowOfflineAccess=true,
                RequirePkce = true,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "userinfo",
                    "openapi",
                    "identityserver",
                    "devicecenter"
                },
                AllowedCorsOrigins = { 
                    "http://localhost:8000",
                    "https://cloud.helloworldnet.com"
                },
                AccessTokenLifetime = 600
            }
        };

        public static IEnumerable<IdentityTenant> Tenants()
        {
            Random random = new(Environment.TickCount);

            var result = new List<IdentityTenant>();

            for (int i = 1; i < 3; i++)
            {
                result.Add(new IdentityTenant()
                {
                    Id = Guid.Parse($"5f6f2110-58b6-4cf9-b416-85820ba12c0{i}"),
                    Name = $"tenant{i}",
                    CreationTime = DateTime.Now.AddMinutes(-random.Next(0, 10000)),
                    NormalizedName = $"tenant{i}".ToUpper(),
                    DisplayName = $"公司租户{i}"
                });
            }

            return result;
        }

        public static IEnumerable<ApplicationRole> Roles()
        {
            var result = new List<ApplicationRole>();

            var tenants = Tenants().ToArray();

            result.Add(new ApplicationRole { Name = AuthorizeConstants.TenantOwnerRequireRole, TenantId = null, TenantRoleName = AuthorizeConstants.TenantOwnerRequireRole, DisplayName = "租户所有者" });

            for (int i = 1; i < 3; i++)
            {
                string tenantRoleName = $"role{i}";
                result.Add(new ApplicationRole { Name = tenantRoleName, TenantId = null, TenantRoleName = tenantRoleName, DisplayName = $"宿主角色{i}" });
            }

            for (int i = 0; i < tenants.Length; i++)
            {
                result.Add(new ApplicationRole
                {
                    Name = $"{AuthorizeConstants.TenantOwnerRequireRole}@{tenants[i].Name}",
                    TenantId = tenants[i].Id,
                    TenantRoleName = AuthorizeConstants.TenantOwnerRequireRole,
                    DisplayName = $"租户所有者{i}"
                });

                for (int j = 1; j < 3; j++)
                {
                    string tenantRoleName = $"role{j}";

                    result.Add(new ApplicationRole
                    {
                        Name = $"{tenantRoleName}@{tenants[i].Name}",
                        TenantId = tenants[i].Id,
                        TenantRoleName = tenantRoleName,
                        DisplayName = $"租户角色{i}"
                    });
                }
            }

            return result;
        }

        public static Dictionary<ApplicationUser, IEnumerable<Claim>> Users()
        {
            var tenants = Tenants().ToArray();
            var roles = Roles().ToArray();

            Random random = new(Environment.TickCount);

            string stringMock() => $"{Path.GetRandomFileName().Replace(".", string.Empty)}";
            string phoneMock() => $"{random.Next(130, 190)}{random.Next(10000000, 99999999)}";
            DateTimeOffset dateMock() => DateTimeOffset.Now.AddDays(-random.Next(1, 365 * 30));

            Dictionary<ApplicationUser, IEnumerable<Claim>> result = new();

            var currentHostRoles = roles.Where(r => r.TenantId == null).ToArray();
            List<Claim> claims = currentHostRoles.Select(r => new Claim(JwtClaimTypes.Role, r.Name!, ClaimValueTypes.String)).ToList();
            claims.Add(new Claim(JwtClaimTypes.BirthDate, dateMock().ToString("yyyy-MM-dd"), ClaimValueTypes.Date));
            var user = new ApplicationUser
            {
                UserName = "admin",
                PasswordHash = "guest",
                PhoneNumber = phoneMock(),
                Email = "admin@helloworldnet.com",
                TenantId = null,
                TenantUserName = "admin",
                DisplayName = "超级管理员"
            };
            result[user] = claims;

            for (int i = 1; i < 3; i++)
            {
                IdentityTenant tenant = tenants[i - 1];
                var currentTenantRoles = roles.Where(r => r.TenantId == tenant.Id).ToArray();
                if (i == 2)
                {
                    currentTenantRoles = currentTenantRoles.Where(r => r.TenantRoleName != AuthorizeConstants.TenantOwnerRequireRole).ToArray();
                }
                claims = currentTenantRoles.Select(r => new Claim(JwtClaimTypes.Role, r.Name!, ClaimValueTypes.String)).ToList();
                claims.Add(new Claim(JwtClaimTypes.BirthDate, dateMock().ToString("yyyy-MM-dd"), ClaimValueTypes.Date));
                user = new ApplicationUser
                {
                    UserName = $"guest{i}@{tenant.Name}",
                    PasswordHash = $"guest",
                    PhoneNumber = phoneMock(),
                    Email = $"guest{i}@helloworldnet.com",
                    TenantId = tenant.Id,
                    TenantUserName = $"guest{i}",
                    DisplayName = $"来宾访客{i}"
                };
                result[user] = claims;
            }

            for (int i = 1; i < 100; i++)
            {
                var tenant = tenants[random.Next(0, tenants.Length)];

                var currentUserRoles = roles.Where(r => r.TenantId == tenant.Id).ToArray();

                string tenantUserName = $"user{i.ToString().PadLeft(2, '0')}";

                claims = new List<Claim>
                {
                    new(JwtClaimTypes.BirthDate, dateMock().ToString("yyyy-MM-dd"), ClaimValueTypes.Date),
                    new(JwtClaimTypes.Role, currentUserRoles[random.Next(0,currentUserRoles.Length)].Name!, ClaimValueTypes.String),
                };

                user = new ApplicationUser
                {
                    UserName = $"{tenantUserName}@{tenant.Name}",
                    PasswordHash = tenantUserName,
                    PhoneNumber = phoneMock(),
                    Email = $"{stringMock()}@helloworldnet.com",
                    TenantId = tenant.Id,
                    TenantUserName = tenantUserName,
                    DisplayName = $"普通用户{i.ToString().PadLeft(2, '0')}"
                };
                result[user] = claims;
            }

            return result;
        }
    }
}