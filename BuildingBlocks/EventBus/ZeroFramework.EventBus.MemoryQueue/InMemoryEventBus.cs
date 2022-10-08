using System.Dynamic;
using System.Text.Json;
using ZeroFramework.EventBus.Abstractions;
using ZeroFramework.EventBus.Events;

namespace ZeroFramework.EventBus.MemoryQueue
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IEventBusSubscriptionsManager _subsManager;

        private readonly IServiceProvider _serviceProvider;

        public InMemoryEventBus(IEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider)
        {
            _subsManager = subsManager;
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            string eventName = @event.GetType().Name;
            string message = JsonSerializer.Serialize(@event, @event.GetType());
            await ProcessEvent(eventName, message);
        }

        public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            _subsManager.AddSubscription<T, TH>();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (_serviceProvider.GetService(subscription.HandlerType) is IDynamicIntegrationEventHandler handler)
                        {
                            dynamic? eventData = JsonSerializer.Deserialize<ExpandoObject>(message);
                            await handler.HandleAsync(eventData);
                        }
                    }
                    else
                    {
                        var handler = _serviceProvider.GetService(subscription.HandlerType);

                        if (handler is not null)
                        {
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            object? integrationEvent = JsonSerializer.Deserialize(message, eventType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                            if (integrationEvent is not null)
                            {
                                Task? task = concreteType.GetMethod("HandleAsync")?.Invoke(handler, new object[] { integrationEvent }) as Task;
                                task ??= Task.CompletedTask;
                                await task;
                            }
                        }
                    }
                }
            }

        }
    }
}