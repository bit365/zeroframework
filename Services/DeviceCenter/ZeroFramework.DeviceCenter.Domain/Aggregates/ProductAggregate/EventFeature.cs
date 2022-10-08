using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate
{
    public class EventFeature : AbstractFeature
    {
        [AllowNull]
        public IEnumerable<DataParameter>? OutputData { get; set; }

        public EventType EventType { get; set; }
    }
}