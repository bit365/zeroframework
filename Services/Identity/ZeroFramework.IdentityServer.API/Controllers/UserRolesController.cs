﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthorizeConstants.TenantOwnerPolicyName, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserRolesController(UserManager<ApplicationUser> userManager) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpGet("{userId}")]
        public async Task<IEnumerable<string>> GetUserRoles(string userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return Enumerable.Empty<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }

        [HttpPut("{userId}")]
        public async Task UpdateUserRoles(string userId, IEnumerable<string> roles)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.AddToRolesAsync(user, roles.Where(r => !userRoles.Contains(r)));
                await _userManager.RemoveFromRolesAsync(user, userRoles.Where(ur => !roles.Contains(ur)));
            }
        }
    }
}