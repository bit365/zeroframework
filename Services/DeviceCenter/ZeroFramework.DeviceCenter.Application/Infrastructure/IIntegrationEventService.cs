using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    public interface IIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);

        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}