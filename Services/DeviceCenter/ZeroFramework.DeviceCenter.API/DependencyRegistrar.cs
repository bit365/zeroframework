using Microsoft.AspNetCore.Authorization;
using ZeroFramework.DeviceCenter.API.Extensions.Authorization;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;

namespace ZeroFramework.DeviceCenter.API
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddWebApiLayer(this IServiceCollection services)
        {
            var exportedTypes = System.Reflection.Assembly.GetExecutingAssembly().ExportedTypes;
            var permissionDefinitionProviders = exportedTypes.Where(t => t.IsAssignableTo(typeof(IStartupFilter)));
            permissionDefinitionProviders.ToList().ForEach(t => services.AddTransient(typeof(IStartupFilter), t));

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddTransient<IPermissionChecker, PermissionChecker>();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddTransient<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddTransient<IAuthorizationHandler, ResourcePermissionRequirementHandler>();

            services.AddHttpContextAccessor();

            //services.AddHostedService<MockSampleWorker>();

            return services;
        }
    }
}