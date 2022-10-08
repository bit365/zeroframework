using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Localization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.EventHandling.Ordering;
using ZeroFramework.DeviceCenter.Application.IntegrationEvents.Events.Ordering;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.EventBus.Abstractions;

namespace ZeroFramework.DeviceCenter.API.Extensions.Hosting
{
    public class CustomStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                if (app.ApplicationServices.GetRequiredService<IWebHostEnvironment>().IsStaging())
                {
                    var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
                    eventBus.SubscribeDynamic<OrderPaymentSucceededDynamicIntegrationEventHandler>(nameof(OrderPaymentSucceededIntegrationEvent));
                    eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
                }

                using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
                {
                    var dataSeedProviders = serviceScope.ServiceProvider.GetServices<IDataSeedProvider>();

                    foreach (IDataSeedProvider dataSeedProvider in dataSeedProviders)
                    {
                        dataSeedProvider.SeedAsync(serviceScope.ServiceProvider).Wait();
                    }
                };

                IStringLocalizerFactory? localizerFactory = app.ApplicationServices.GetService<IStringLocalizerFactory>();

                FluentValidation.ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, lambdaExpression) =>
                {
                    string? displayName = string.Empty;

                    DisplayAttribute? displayColumnAttribute = memberInfo.GetCustomAttributes(true).OfType<DisplayAttribute>().FirstOrDefault();

                    if (displayColumnAttribute is not null)
                    {
                        displayName = displayColumnAttribute.Name;
                    }

                    DisplayNameAttribute? displayNameAttribute = memberInfo.GetCustomAttributes(true).OfType<DisplayNameAttribute>().FirstOrDefault();

                    if (displayNameAttribute is not null)
                    {
                        displayName = displayNameAttribute.DisplayName;
                    }

                    if (!string.IsNullOrWhiteSpace(displayName) && localizerFactory is not null)
                    {
                        return localizerFactory.Create(type)[displayName];
                    }

                    if (!string.IsNullOrWhiteSpace(displayName))
                    {
                        return displayName;
                    }

                    return memberInfo.Name;
                };

                app.UseCors();

                app.UseRequestLocalization();

                //Configure ASP.NET Core to work with proxy servers and load balancers
                app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

                next(app);
            };
        }
    }
}