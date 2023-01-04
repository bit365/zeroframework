using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ZeroFramework.DeviceCenter.Application.Behaviors;
using ZeroFramework.DeviceCenter.Application.Infrastructure;
using ZeroFramework.DeviceCenter.Application.Models.Monitoring;
using ZeroFramework.DeviceCenter.Application.Models.Projects;
using ZeroFramework.DeviceCenter.Application.Queries.Factories;
using ZeroFramework.DeviceCenter.Application.Queries.Monitoring;
using ZeroFramework.DeviceCenter.Application.Queries.Ordering;
using ZeroFramework.DeviceCenter.Application.Services.Devices;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;
using ZeroFramework.DeviceCenter.Application.Services.Ordering;
using ZeroFramework.DeviceCenter.Application.Services.Permissions;
using ZeroFramework.DeviceCenter.Application.Services.Products;
using ZeroFramework.DeviceCenter.Application.Services.Projects;
using ZeroFramework.DeviceCenter.Application.Services.ResourceGroups;
using ZeroFramework.DeviceCenter.Application.Services.Tenants;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MonitoringAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Infrastructure.Constants;
using ZeroFramework.DeviceCenter.Infrastructure.Idempotency;
using ZeroFramework.EventBus;
using ZeroFramework.EventBus.Abstractions;
using ZeroFramework.EventBus.RabbitMQ;

namespace ZeroFramework.DeviceCenter.Application
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomainEvents();
            services.AddEventBus(configuration).AddIntegrationEvents();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddQueries(configuration);
            services.AddApplicationServices();
            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection AddDomainEvents(this IServiceCollection services)
        {
            // Registers handlers and mediator types from the specified assemblies
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            ValidatorOptions.Global.LanguageManager = new Extensions.Validators.CustomLanguageManager();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddTransient<IRequestManager, RequestManager>();

            return services;
        }

        private static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            IConfigurationSection configurationSection = configuration.GetSection("EventBus");
            int retryCount = 5;
            if (!string.IsNullOrEmpty(configurationSection["EventBusRetryCount"]))
            {
                retryCount = int.Parse(configurationSection["EventBusRetryCount"]!);
            }

            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new RabbitMQ.Client.ConnectionFactory()
                {
                    HostName = configurationSection["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configurationSection["EventBusUserName"]))
                {
                    factory.UserName = configurationSection["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configurationSection["EventBusPassword"]))
                {
                    factory.Password = configurationSection["EventBusPassword"];
                }

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });

            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                string queueName = configurationSection["SubscriptionClientName"]!;
                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, queueName, retryCount);
            });

            return services;
        }

        private static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            var exportedTypes = Assembly.GetExecutingAssembly().ExportedTypes;

            var integrationEventHandlers = exportedTypes.Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)) && t.IsClass);
            integrationEventHandlers.ToList().ForEach(t => services.AddTransient(typeof(IIntegrationEventHandler), t));

            services.AddTransient<IIntegrationEventService, IntegrationEventService>();

            return services;
        }

        private static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

            string connectionString = configuration.GetConnectionString(DbConstants.DefaultConnectionStringName)!;
            services.AddTransient<IOrderQueries>(o => new OrderQueries(connectionString));
            services.AddTransient<IMonitoringFactorQueries, MonitoringFactorQueries>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var exportedTypes = Assembly.GetExecutingAssembly().ExportedTypes;

            var dataSeedProviders = exportedTypes.Where(t => t.IsAssignableTo(typeof(IDataSeedProvider)) && t.IsClass);
            dataSeedProviders.ToList().ForEach(t => services.AddTransient(typeof(IDataSeedProvider), t));

            services.AddTransient(typeof(ICrudApplicationService<int, ProjectGetResponseModel, PagedRequestModel, ProjectGetResponseModel, ProjectCreateOrUpdateRequestModel, ProjectCreateOrUpdateRequestModel>), typeof(ProjectApplicationService));
            services.AddTransient(typeof(ICrudApplicationService<int, MonitoringFactorGetResponseModel, MonitoringFactorPagedRequestModel, MonitoringFactorGetResponseModel, MonitoringFactorCreateRequestModel, MonitoringFactorUpdateRequestModel>), typeof(CrudApplicationService<MonitoringFactor, int, MonitoringFactorGetResponseModel, MonitoringFactorPagedRequestModel, MonitoringFactorGetResponseModel, MonitoringFactorCreateRequestModel, MonitoringFactorUpdateRequestModel>));
            services.AddTransient<IOrderApplicationService, OrderApplicationService>();
            services.AddTransient<IProductApplicationService, ProductApplicationService>();
            services.AddTransient<ITenantApplicationService, TenantApplicationService>();
            services.AddTransient<IPermissionApplicationService, PermissionApplicationService>();
            services.AddTransient<IResourceGroupApplicationService, ResourceGroupApplicationService>();
            services.AddTransient<IMeasurementUnitApplicationService, MeasurementUnitApplicationService>();
            services.AddTransient<IDeviceApplicationService, DeviceApplicationService>();
            services.AddTransient<IDeviceDataApplicationService, DeviceDataApplicationService>();
            services.AddTransient<IDeviceGroupApplicationService, DeviceGroupApplicationService>();

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache().AddTransient<IPermissionStore, PermissionStore>();
            services.AddTransient<IPermissionDefinitionManager, PermissionDefinitionManager>();

            var exportedTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.ExportedTypes).Where(t => t.IsClass);

            var permissionDefinitionProviders = exportedTypes.Where(t => t.IsAssignableTo(typeof(IPermissionDefinitionProvider)));
            permissionDefinitionProviders.ToList().ForEach(t => services.AddSingleton(typeof(IPermissionDefinitionProvider), t));

            var permissionValueProviders = exportedTypes.Where(t => t.IsAssignableTo(typeof(IPermissionValueProvider)));
            permissionValueProviders.ToList().ForEach(t => services.AddTransient(typeof(IPermissionValueProvider), t));

            return services;
        }
    }
}