using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace ZeroFramework.DeviceCenter.Infrastructure.IntegrationEvents
{
    public class IntegrationEventLog : Domain.Entities.BaseEntity<Guid>
    {
        private IntegrationEventLog() { }

        public IntegrationEventLog(Guid id, object @event, Guid? transactionId)
        {
            Id = id;
            CreationTime = DateTimeOffset.Now;
            EventTypeName = @event.GetType().FullName ?? string.Empty;
            Content = JsonSerializer.Serialize(@event, @event.GetType());
            Status = IntegrationEventStatus.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId?.ToString();
        }

        public string EventTypeName { get; private set; } = string.Empty;

        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last() ?? string.Empty;

        public IntegrationEventStatus Status { get; set; }

        public int TimesSent { get; set; }

        public DateTimeOffset CreationTime { get; private set; }

        public string Content { get; private set; } = string.Empty;

        public string? TransactionId { get; private set; }
    }
}
