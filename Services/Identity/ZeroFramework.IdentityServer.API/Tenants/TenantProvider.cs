using ZeroFramework.IdentityServer.API.Constants;
using ZeroFramework.IdentityServer.API.IdentityStores;

namespace ZeroFramework.IdentityServer.API.Tenants
{
    public class TenantProvider : ITenantProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApplicationDbContext _dbContext;

        public TenantProvider(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public virtual async Task<IdentityTenant?> GetTenantAsync()
        {
            string? tenantIdOrName = ResolveTenantIdOrName(_httpContextAccessor.HttpContext ?? new DefaultHttpContext());

            if (tenantIdOrName is not null)
            {
                return await FindTenantAsync(tenantIdOrName);
            }

            return null;
        }

        public virtual async Task<IdentityTenant?> FindTenantAsync(string tenantIdOrName)
        {
            if (Guid.TryParse(tenantIdOrName, out var parsedTenantId))
            {
                return await _dbContext.FindAsync<IdentityTenant>(parsedTenantId);
            }

            return _dbContext.Set<IdentityTenant>().Single(t => t.Name == tenantIdOrName);
        }

        public virtual string? ResolveTenantIdOrName(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue(TenantConstants.TenantKey, out var headerValues))
            {
                return headerValues.First();
            }

            if (httpContext.Request.Query.TryGetValue(TenantConstants.TenantKey, out var queryValues))
            {
                return queryValues.First();
            }

            if (httpContext.Request.Cookies.TryGetValue(TenantConstants.TenantKey, out var cookieValue))
            {
                return cookieValue;
            }

            if (httpContext.Request.RouteValues.TryGetValue(TenantConstants.TenantKey, out var routeValue))
            {
                return routeValue?.ToString();
            }

            return httpContext.User.FindFirst(TenantConstants.TenantKey)?.Value;
        }
    }
}
