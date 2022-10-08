using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Users;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthorizeConstants.TenantOwnerPolicyName, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserClaimsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserClaimsController(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserClaimModel>>> GetUserClaims(int userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);

            return claims.Select(c => new UserClaimModel(c.Type, c.Value)).ToList();
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult<IEnumerable<UserClaimModel>>> PostUserClaims(int userId, IEnumerable<UserClaimModel> userClaims)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());
            IList<Claim> claims = await _userManager.GetClaimsAsync(user);
            var newClaims = userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue));

            newClaims = newClaims.Where(nc => !claims.Any(c => c.Type == nc.Type && c.Value == nc.Value));

            IdentityResult identityResult = await _userManager.AddClaimsAsync(user, newClaims);

            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                return ValidationProblem(ModelState);
            }

            return CreatedAtAction("GetUserClaims", new { userId = user.Id }, userClaims);
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUserClaims(int userId, IEnumerable<UserClaimModel> userClaims)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId.ToString());

            var claims = userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue));

            IdentityResult identityResult = await _userManager.RemoveClaimsAsync(user, claims);

            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                return ValidationProblem(ModelState);
            }

            return NoContent();
        }
    }
}
