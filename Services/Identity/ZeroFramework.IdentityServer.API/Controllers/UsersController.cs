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
using ZeroFramework.IdentityServer.API.Models.Users;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthorizeConstants.TenantOwnerPolicyName, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMapper _mapper;

        private readonly ICurrentTenant _currentTenant;

        public UsersController(UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentTenant currentTenant)
        {
            _userManager = userManager;
            _mapper = mapper;
            _currentTenant = currentTenant;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<PagedResponseModel<UserGetResponseModel>>> GetUsers(string keyword, [ModelBinder(typeof(SortingModelBinder))] IEnumerable<SortingDescriptor>? sorter, int pageNumber = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            IQueryable<ApplicationUser> userQuery = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                userQuery = _userManager.Users.Where(u => u.UserName.Contains(keyword) || u.PhoneNumber.Contains(keyword) || (u.DisplayName != null && u.DisplayName.Contains(keyword)));
            }

            int totalCount = await userQuery.CountAsync();

            userQuery = sorter != null && sorter.Any() ? userQuery.ApplySorting(sorter) : userQuery.OrderByDescending(e => e.CreationTime);
            userQuery = userQuery.ApplyPaging(pageNumber, pageSize);

            List<UserGetResponseModel> list = _mapper.Map<List<UserGetResponseModel>>(await userQuery.ToListAsync());

            return new PagedResponseModel<UserGetResponseModel>(list, totalCount);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserGetResponseModel>> GetUser(int id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return NotFound();
            }

            return _mapper.Map<UserGetResponseModel>(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserGetResponseModel>> PostUser(UserCreateRequestModel userModel)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(userModel);
            user.TenantUserName = user.UserName;
            user.UserName = _currentTenant.Name is null ? user.TenantUserName : $"{user.TenantUserName}@{_currentTenant.Name}";

            IdentityResult identityResult = await _userManager.CreateAsync(user, userModel.Password);

            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                return ValidationProblem(ModelState);
            }

            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);
            identityResult = await _userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);

            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                return ValidationProblem(ModelState);
            }

            var model = _mapper.Map<UserGetResponseModel>(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, model);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateRequestModel userModel)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());

            _mapper.Map(userModel, user);

            user.TenantUserName = user.UserName;
            user.UserName = _currentTenant.Name is null ? user.TenantUserName : $"{user.TenantUserName}@{_currentTenant.Name}";
            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userModel.Password);

            IdentityResult identityResult = await _userManager.UpdateAsync(user);

            if (!identityResult.Succeeded)
            {
                identityResult.Errors.ToList().ForEach(e => ModelState.AddModelError(string.Empty, e.Description));
                return ValidationProblem(ModelState);
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id.ToString());

            if (user is null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return NoContent();
        }
    }
}