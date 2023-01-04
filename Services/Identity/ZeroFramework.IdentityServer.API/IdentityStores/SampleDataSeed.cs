using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.IdentityStores
{
    public static class SampleDataSeed
    {
        public static async Task SeedAsync(IApplicationBuilder app)
        {
            await EnsureDeletedMigrateAsync(app);
            await SeedIdentityServerDatasAsync(app);
            await SeedIdentityDatasAsync(app);
        }

        private static async Task EnsureDeletedMigrateAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //await persistedGrantDbContext.Database.EnsureDeletedAsync();
            //await configurationDbContext.Database.EnsureDeletedAsync();
            //await applicationDbContext.Database.EnsureDeletedAsync();

            await persistedGrantDbContext.Database.MigrateAsync();
            await configurationDbContext.Database.MigrateAsync();
            await applicationDbContext.Database.MigrateAsync();
        }

        private static async Task SeedIdentityServerDatasAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!configurationDbContext.Clients.Any())
            {
                await configurationDbContext.Clients.AddRangeAsync(SampleDatas.Clients.Select(c => c.ToEntity()));
                await configurationDbContext.SaveChangesAsync();
            }

            if (!configurationDbContext.IdentityResources.Any())
            {
                await configurationDbContext.IdentityResources.AddRangeAsync(SampleDatas.Ids.Select(id => id.ToEntity()));
                await configurationDbContext.SaveChangesAsync();
            }

            if (!configurationDbContext.ApiResources.Any())
            {
                await configurationDbContext.ApiResources.AddRangeAsync(SampleDatas.Apis.Select(apir => apir.ToEntity()));
                await configurationDbContext.SaveChangesAsync();
            }

            if (!configurationDbContext.ApiScopes.Any())
            {
                await configurationDbContext.ApiScopes.AddRangeAsync(SampleDatas.ApiScopes.Select(apis => apis.ToEntity()));
                await configurationDbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedIdentityDatasAsync(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (!applicationDbContext.Set<IdentityTenant>().Any())
            {
                await applicationDbContext.AddRangeAsync(SampleDatas.Tenants());
                await applicationDbContext.SaveChangesAsync();
            }

            var currentTenant = serviceScope.ServiceProvider.GetRequiredService<ICurrentTenant>();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (!roleManager.Roles.Any())
            {
                foreach (var role in SampleDatas.Roles())
                {
                    using (currentTenant.Change(role.TenantId))
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (!userManager.Users.Any())
            {
                foreach (var user in SampleDatas.Users())
                {
                    ApplicationUser createdUser = user.Key;

                    IEnumerable<Claim> userClaims = user.Value;

                    using (currentTenant.Change(user.Key.TenantId))
                    {
                        if (createdUser.PasswordHash != null)
                        {
                            await userManager.CreateAsync(createdUser, createdUser.PasswordHash);
                        }

                        await userManager.ConfirmEmailAsync(createdUser, await userManager.GenerateEmailConfirmationTokenAsync(createdUser));

                        if (createdUser.PhoneNumber != null)
                        {
                            await userManager.ChangePhoneNumberAsync(createdUser, createdUser.PhoneNumber, await userManager.GenerateChangePhoneNumberTokenAsync(createdUser, createdUser.PhoneNumber));
                        }

                        var userRoleClaims = userClaims.Where(t => t.Type == JwtClaimTypes.Role || t.Type == ClaimTypes.Role);

                        await userManager.AddClaimsAsync(createdUser, userClaims.Except(userRoleClaims));

                        var userRoleNames = userRoleClaims?.Select(urc => urc.Value);

                        if (userRoleNames != null && userRoleNames.Any())
                        {
                            await userManager.AddToRolesAsync(createdUser, userRoleNames);
                        }
                    }
                }
            }
        }
    }
}