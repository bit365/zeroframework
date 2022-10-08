using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Infrastructure.IntegrationEvents;
using ZeroFramework.EventBus.Abstractions;
using ZeroFramework.EventBus.Events;

namespace ZeroFramework.DeviceCenter.Application.Infrastructure
{
    public class IntegrationEventService : IIntegrationEventService
    {
        private readonly IEventBus _eventBus;

        private readonly IRepository<IntegrationEventLog> _eventLogRepository;

        private readonly ILogger<IntegrationEventService> _logger;

        private readonly List<Type> _eventTypes;

        public IntegrationEventService(IEventBus eventBus, IRepository<IntegrationEventLog> eventLogRepository, ILogger<IntegrationEventService> logger)
        {
            _eventBus = eventBus;
            _eventLogRepository = eventLogRepository;
            _logger = logger;
            _eventTypes = Assembly.GetExecutingAssembly().ExportedTypes.Where(t => t.Name.EndsWith(nameof(IntegrationEvent))).ToList();
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var tid = transactionId.ToString();

            IEnumerable<IntegrationEventLog> result = await _eventLogRepository.Query.Where(e => e.TransactionId == tid && e.Status == IntegrationEventStatus.NotPublished).ToListAsync();

            List<IntegrationEvent> pendingLogEvents = new();

            if (result != null && result.Any())
            {
                result = result.OrderBy(o => o.CreationTime);

                foreach (var item in result)
                {
                    Type? eventType = _eventTypes.Find(t => t.Name == item.EventTypeShortName);
                    if (eventType is not null)
                    {
                        IntegrationEvent? integrationEvent = JsonSerializer.Deserialize(item.Content, eventType) as IntegrationEvent;
                        if (integrationEvent is not null)
                        {
                            pendingLogEvents.Add(integrationEvent);
                        }
                    }
                }
            }

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.Id, "ZeroFramework", logEvt);

                try
                {
                    await UpdateEventStatus(logEvt.Id, IntegrationEventStatus.InProgress);
                    await _eventBus.PublishAsync(logEvt);
                    await UpdateEventStatus(logEvt.Id, IntegrationEventStatus.Published);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.Id, "ZeroFramework");
                    await UpdateEventStatus(logEvt.Id, IntegrationEventStatus.PublishedFailed);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            Guid? transactionId = System.Transactions.Transaction.Current?.TransactionInformation.DistributedIdentifier;

            await _eventLogRepository.InsertAsync(new IntegrationEventLog(evt.Id, evt, transactionId), true);
        }

        private async Task UpdateEventStatus(Guid eventId, IntegrationEventStatus status)
        {
            IntegrationEventLog eventLog = await _eventLogRepository.GetAsync(ev => ev.Id == eventId);

            eventLog.Status = status;

            if (status == IntegrationEventStatus.InProgress)
            {
                eventLog.TimesSent++;
            }

            await _eventLogRepository.UpdateAsync(eventLog, true);
        }
    }
}