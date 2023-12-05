using ZeroFramework.DeviceCenter.API.Constants;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;

namespace ZeroFramework.DeviceCenter.API.Extensions.Tenants
{
    public class TenantMiddleware(ICurrentTenant currentTenant) : IMiddleware
    {
        private readonly ICurrentTenant _currentTenant = currentTenant;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string? tenantIdString = ResolveTenantId(context);
            if (Guid.TryParse(tenantIdString, out var parsedTenantId))
            {
                using (_currentTenant.Change(parsedTenantId))
                {
                    await next(context);
                }
            }
            else
            {
                await next(context);
            }
        }

        protected virtual string? ResolveTenantId(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue(TenantClaimTypes.TenantId, out var headerValues))
            {
                return headerValues.First();
            }

            if (httpContext.Request.Query.TryGetValue(TenantClaimTypes.TenantId, out var queryValues))
            {
                return queryValues.First();
            }

            if (httpContext.Request.Cookies.TryGetValue(TenantClaimTypes.TenantId, out var cookieValue))
            {
                return cookieValue;
            }

            if (httpContext.Request.RouteValues.TryGetValue(TenantClaimTypes.TenantId, out var routeValue))
            {
                return routeValue?.ToString();
            }

            return httpContext.User.FindFirst(TenantClaimTypes.TenantId)?.Value;
        }
    }
}
