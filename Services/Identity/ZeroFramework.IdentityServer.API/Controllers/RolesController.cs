using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Extensions;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Generics;
using ZeroFramework.IdentityServer.API.Models.Roles;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthorizeConstants.TenantOwnerPolicyName, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController(RoleManager<ApplicationRole> roleManager, IMapper mapper, ICurrentTenant currentTenant) : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

        private readonly IMapper _mapper = mapper;

        private readonly ICurrentTenant _currentTenant = currentTenant;

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<PagedResponseModel<RoleGetResponseModel>>> GetRoles(string keyword, [FromQuery] PagedRequestModel model)
        {
            IQueryable<ApplicationRole> roleQuery = _roleManager.Roles;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                roleQuery = _roleManager.Roles.Where(u => u.Name!.Contains(keyword) || (u.DisplayName != null && u.DisplayName.Contains(keyword)));
            }

            int totalCount = await _roleManager.Roles.CountAsync();

            roleQuery = roleQuery.ApplySortingAndPaging(model);

            List<RoleGetResponseModel> list = _mapper.Map<List<RoleGetResponseModel>>(await roleQuery.ToListAsync());

            return new PagedResponseModel<RoleGetResponseModel>(list, totalCount);
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleGetResponseModel>> GetRole(int id)
        {
            ApplicationRole? role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
            {
                return NotFound();
            }

            return _mapper.Map<RoleGetResponseModel>(role);
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<RoleGetResponseModel>> PostRole(RoleCreateRequestModel roleModel)
        {
            ApplicationRole role = _mapper.Map<ApplicationRole>(roleModel);

            role.TenantRoleName = roleModel.RoleName;
            role.Name = _currentTenant.Name is null ? role.TenantRoleName : $"{role.TenantRoleName}@{_currentTenant.Name}";

            IdentityResult identityResult = await _roleManager.CreateAsync(role);

            if (await _roleManager.RoleExistsAsync(roleModel.RoleName))

                if (!identityResult.Succeeded)
                {
                    identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                    return ValidationProblem(ModelState);
                }

            return CreatedAtAction("GetRole", new { id = role.Id }, _mapper.Map<RoleGetResponseModel>(role));
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleUpdateRequestModel roleModel)
        {
            ApplicationRole? role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
            {
                return NotFound();
            }

            _mapper.Map(roleModel, role);

            IdentityResult identityResult = await _roleManager.UpdateAsync(role);
            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                return ValidationProblem(ModelState);
            }

            return NoContent();
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            ApplicationRole? role = await _roleManager.FindByIdAsync(id.ToString());

            if (role is null)
            {
                return NotFound();
            }

            await _roleManager.DeleteAsync(role);

            return NoContent();
        }
    }
}
