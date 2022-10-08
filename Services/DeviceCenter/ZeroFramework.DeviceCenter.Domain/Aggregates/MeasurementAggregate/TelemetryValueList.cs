using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class TelemetryValueList
    {
        [AllowNull]
        public Guid ProductId { get; set; }

        public long DeviceId { get; set; }

        [AllowNull]
        public List<TelemetryValue> Values { get; set; } = new();
    }
}
