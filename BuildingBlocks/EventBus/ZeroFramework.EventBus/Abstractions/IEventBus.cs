using ZeroFramework.EventBus.Events;

namespace ZeroFramework.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);

        void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;

        void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler;
    }
}
