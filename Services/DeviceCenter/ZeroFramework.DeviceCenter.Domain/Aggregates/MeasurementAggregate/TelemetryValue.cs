using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class TelemetryValue
    {
        [AllowNull]
        public string Identifier { get; set; }

        public long Timestamp { get; set; }

        [AllowNull]
        public object Value { get; set; }
    }
}