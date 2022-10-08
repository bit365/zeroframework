namespace ZeroFramework.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler : IIntegrationEventHandler
    {
        Task HandleAsync(dynamic eventData);
    }
}
