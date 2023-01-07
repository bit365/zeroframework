using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using ZeroFramework.DeviceCenter.Domain.Aggregates.BuyerAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.OrderAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.PermissionAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Infrastructure.ConnectionStrings;
using ZeroFramework.DeviceCenter.Infrastructure.Constants;
using ZeroFramework.DeviceCenter.Infrastructure.EntityFrameworks;
using ZeroFramework.DeviceCenter.Infrastructure.Repositories;

namespace ZeroFramework.DeviceCenter.Infrastructure
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConnectionStringProvider, TenantConnectionStringProvider>();

            services.AddEntityFrameworkSqlServer();

            bool isDemoMode = Convert.ToBoolean(configuration.GetRequiredSection("UseDemoLaunchMode").Value);

            services.AddDbContext<DeviceCenterDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DefaultConnectionStringName), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });

                var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
                optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringProvider));

                if (isDemoMode)
                {
                    optionsBuilder.AddInterceptors(new DisalbeModifiedDeletedSaveChangesInterceptor());
                }

                IMediator mediator = serviceProvider.GetService<IMediator>() ?? new NullMediator();

                optionsBuilder.AddInterceptors(new CustomSaveChangesInterceptor(mediator));

                optionsBuilder.AddInterceptors(new CustomDbCommandInterceptor());

                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            }, ServiceLifetime.Transient, ServiceLifetime.Singleton);

            services.AddPooledDbContextFactory<DeviceCenterDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString(DbConstants.DefaultConnectionStringName), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });

                var connectionStringProvider = serviceProvider.GetRequiredService<IConnectionStringProvider>();
                optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringProvider));

                if (isDemoMode)
                {
                    optionsBuilder.AddInterceptors(new DisalbeModifiedDeletedSaveChangesInterceptor());
                }

                IMediator mediator = serviceProvider.GetService<IMediator>() ?? new NullMediator();
                optionsBuilder.AddInterceptors(new CustomSaveChangesInterceptor(mediator));

                optionsBuilder.AddInterceptors(new CustomDbCommandInterceptor());

                optionsBuilder.UseInternalServiceProvider(serviceProvider);
            });

            services.AddTransient(typeof(IRepository<>), typeof(DeviceCenterEfCoreRepository<>));
            services.AddTransient(typeof(IRepository<,>), typeof(DeviceCenterEfCoreRepository<,>));

            services.AddTransient<IBuyerRepository, BuyerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IPermissionGrantRepository, PermissionGrantRepository>();
            services.AddTransient<IMeasurementRepository, MeasurementRepository>();
            services.AddTransient<IDeviceRepository, DeviceRepository>();

            services.Configure<IncludeRelatedPropertiesOptions>(options =>
            {
                options.ConfigIncludes<DeviceGroup>(e => e.Include(e => e.Devices));
            });

            return services;
        }
    }
}