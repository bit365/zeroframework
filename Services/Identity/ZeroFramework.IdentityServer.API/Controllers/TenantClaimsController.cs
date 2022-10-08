using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.IdentityStores;
using ZeroFramework.IdentityServer.API.Models.Tenants;

namespace ZeroFramework.IdentityServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = AuthorizeConstants.TenantOwnerRequireRole, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TenantClaimsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public TenantClaimsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/TenantClaims
        [HttpGet]
        [HttpGet("{tenantId:guid}")]
        public async Task<ActionResult<IEnumerable<TenantClaimModel>>> GetTenantClaims(Guid tenantId)
        {
            IQueryable<IdentityTenantClaim> query = _context.Set<IdentityTenantClaim>().Where(e => e.TenantId == tenantId);
            return _mapper.Map<List<TenantClaimModel>>(await query.ToListAsync());
        }

        // GET: api/TenantClaims/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantClaimModel>> GetTenantClaim(int id)
        {
            IdentityTenantClaim? identityTenantClaim = await _context.Set<IdentityTenantClaim>().FindAsync(id);

            if (identityTenantClaim is null)
            {
                return NotFound();
            }

            return _mapper.Map<TenantClaimModel>(identityTenantClaim);
        }

        // PUT: api/TenantClaims/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenantClaim(int id, TenantClaimModel tenantClaimModel)
        {
            if (id != tenantClaimModel.Id)
            {
                return BadRequest();
            }

            IdentityTenantClaim identityTenantClaim = _mapper.Map<IdentityTenantClaim>(tenantClaimModel);

            _context.Entry(identityTenantClaim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenantClaimExists(id))
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

        // POST: api/TenantClaims
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IdentityTenantClaim>> PostTenantClaim(TenantClaimModel tenantClaimModel)
        {
            IdentityTenantClaim identityTenantClaim = _mapper.Map<IdentityTenantClaim>(tenantClaimModel);

            _context.Set<IdentityTenantClaim>().Add(identityTenantClaim);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTenantClaim), new { id = tenantClaimModel.Id }, tenantClaimModel);
        }

        // DELETE: api/TenantClaims/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenantClaim(int id)
        {
            var identityTenantClaim = await _context.Set<IdentityTenantClaim>().FindAsync(id);
            if (identityTenantClaim == null)
            {
                return NotFound();
            }

            _context.Set<IdentityTenantClaim>().Remove(identityTenantClaim);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TenantClaimExists(int id)
        {
            return _context.Set<IdentityTenantClaim>().Any(e => e.Id == id);
        }
    }
}
