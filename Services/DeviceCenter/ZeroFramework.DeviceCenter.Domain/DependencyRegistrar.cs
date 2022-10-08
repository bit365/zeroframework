using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;
using ZeroFramework.DeviceCenter.Domain.Services.Devices;
using ZeroFramework.DeviceCenter.Domain.Services.Ordering;

namespace ZeroFramework.DeviceCenter.Domain
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<IOrderDomainService, OrderDomainService>();
            services.AddTransient<IDeviceGroupDomainService, DeviceGroupDomainService>();

            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();

            TypeDescriptor.AddAttributes(typeof(GeoCoordinate), new TypeConverterAttribute(typeof(GeoCoordinateTypeConverter)));

            return services;
        }
    }
}