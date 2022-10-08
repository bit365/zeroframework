using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.Extensions;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Generics;
using ZeroFramework.IdentityServer.API.Models.Tenants;
using ZeroFramework.IdentityServer.API.Tenants;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthorizeConstants.TenantOwnerRequireRole, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TenantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly IStringLocalizer<TenantsController> _stringLocalizer;

        public TenantsController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IStringLocalizer<TenantsController> stringLocalizer)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _stringLocalizer = stringLocalizer;
        }

        // GET: api/Tenants
        [HttpGet]
        public async Task<ActionResult<PagedResponseModel<TenantGetResponseModel>>> GetTenants(string keyword, [ModelBinder(typeof(SortingModelBinder))] IEnumerable<SortingDescriptor> sorter, int pageNumber = 1, int pageSize = PagingConstants.DefaultPageSize)
        {
            IQueryable<IdentityTenant> query = _context.Set<IdentityTenant>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(u => u.Name.Contains(keyword));
            }

            int totalCount = await query.CountAsync();

            query = sorter.Any() ? query.ApplySorting(sorter) : query.OrderByDescending(e => e.CreationTime);

            query = query.ApplyPaging(pageNumber, pageSize);

            List<TenantGetResponseModel> list = _mapper.Map<List<TenantGetResponseModel>>(await query.ToListAsync());

            return new PagedResponseModel<TenantGetResponseModel>(list, totalCount);
        }

        // GET: api/Tenants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantGetResponseModel>> GetTenant(Guid id)
        {
            IdentityTenant? identityTenant = await _context.Set<IdentityTenant>().FindAsync(id);

            if (identityTenant == null)
            {
                return NotFound();
            }

            TenantGetResponseModel tenantModel = _mapper.Map<TenantGetResponseModel>(identityTenant);
            tenantModel.ConnectionString = (await _context.Set<IdentityTenantClaim>().SingleOrDefaultAsync(tc => tc.TenantId == id && tc.ClaimType == nameof(tenantModel.ConnectionString)))?.ClaimValue;

            return tenantModel;
        }

        // PUT: api/Tenants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenant(Guid id, TenantUpdateRequestModel tenantModel)
        {
            if (id != tenantModel.Id)
            {
                return BadRequest();
            }

            IdentityTenant? identityTenant = await _context.FindAsync<IdentityTenant>(id);

            if (identityTenant is null)
            {
                return NotFound();
            }

            _mapper.Map(tenantModel, identityTenant);
            _context.Entry(identityTenant).State = EntityState.Modified;

            var identityTenantClaim = await _context.Set<IdentityTenantClaim>().SingleOrDefaultAsync(tc => tc.TenantId == id && tc.ClaimType == nameof(tenantModel.ConnectionString));

            if (!string.IsNullOrWhiteSpace(tenantModel.ConnectionString))
            {
                if (identityTenantClaim is not null)
                {
                    identityTenantClaim.ClaimValue = tenantModel.ConnectionString;
                    _context.Update(identityTenantClaim);

                }
                else
                {
                    identityTenantClaim = new()
                    {
                        ClaimType = nameof(tenantModel.ConnectionString),
                        ClaimValue = tenantModel.ConnectionString,
                        TenantId = identityTenant.Id
                    };
                    _context.Add(identityTenantClaim);
                }
            }
            else
            {
                if (identityTenantClaim is not null)
                {
                    _context.Remove(identityTenantClaim);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tenants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TenantCreateRequestModel>> PostTenant(TenantCreateRequestModel tenantModel, [FromServices] ICurrentTenant currentTenant)
        {
            if (await _context.Set<IdentityTenant>().AnyAsync(t => t.Name == tenantModel.Name))
            {
                return ValidationProblem(title: _stringLocalizer["Tenant name already exists"]);
            }

            IdentityTenant identityTenant = _mapper.Map<IdentityTenant>(tenantModel);
            identityTenant.NormalizedName = tenantModel.Name.ToUpper();
            _context.Set<IdentityTenant>().Add(identityTenant);

            if (!string.IsNullOrWhiteSpace(tenantModel.ConnectionString))
            {
                IdentityTenantClaim identityTenantClaim = new()
                {
                    ClaimType = nameof(tenantModel.ConnectionString),
                    ClaimValue = tenantModel.ConnectionString,
                    TenantId = identityTenant.Id
                };
                _context.Add(identityTenantClaim);
            }

            await _context.SaveChangesAsync();

            string tenantOwnerRequireRole = AuthorizeConstants.TenantOwnerRequireRole;

            ApplicationRole tenantAdminRole = new()
            {
                TenantRoleName = tenantOwnerRequireRole,
                Name = $"{tenantOwnerRequireRole}@{identityTenant.Name}",
                TenantId = identityTenant.Id,
                DisplayName = tenantOwnerRequireRole
            };

            IdentityResult identityResult = await _roleManager.CreateAsync(tenantAdminRole);

            if (identityResult.Succeeded)
            {
                ApplicationUser tenantAdminUser = new()
                {
                    TenantUserName = tenantModel.AdminUserName,
                    UserName = $"{tenantModel.AdminUserName}@{identityTenant.Name}",
                    TenantId = identityTenant.Id,
                    DisplayName = "Administraotr",
                };

                identityResult = await _userManager.CreateAsync(tenantAdminUser, tenantModel.AdminPassword);

                if (identityResult.Succeeded)
                {
                    using (currentTenant.Change(identityTenant.Id, identityTenant.Name))
                    {
                        await _userManager.AddToRoleAsync(tenantAdminUser, tenantAdminRole.Name);
                    }
                }
            }

            return CreatedAtAction(nameof(GetTenant), new { id = identityTenant.Id }, tenantModel);
        }

        // DELETE: api/Tenants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(Guid id, [FromServices] ICurrentTenant currentTenant)
        {
            var identityTenant = await _context.Set<IdentityTenant>().FindAsync(id);
            if (identityTenant == null)
            {
                return NotFound();
            }

            using (currentTenant.Change(identityTenant.Id, identityTenant.Name))
            {
                _context.RemoveRange(_context.Set<ApplicationUser>());
                _context.RemoveRange(_context.Set<ApplicationRole>());
                _context.RemoveRange(_context.Set<IdentityTenantClaim>());
                _context.Remove(identityTenant);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool TenantExists(Guid id)
        {
            return _context.Set<IdentityTenant>().Any(e => e.Id == id);
        }
    }
}
