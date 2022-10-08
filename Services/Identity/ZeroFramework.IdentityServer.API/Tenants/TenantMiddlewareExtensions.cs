namespace ZeroFramework.IdentityServer.API.Tenants
{
    public static class TenantMiddlewareExtensions
    {
        public static IServiceCollection AddTenantMiddleware(this IServiceCollection services)
        {
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<ITenantProvider, TenantProvider>();

            return services.AddTransient<TenantMiddleware>();
        }

        public static IApplicationBuilder UseTenantMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TenantMiddleware>();
        }
    }
}
