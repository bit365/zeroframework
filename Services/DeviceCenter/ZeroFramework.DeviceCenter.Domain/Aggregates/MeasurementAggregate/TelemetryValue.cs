namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class TelemetryValue
    {
        public string Identifier { get; set; } = default!;

        public long Timestamp { get; set; }

        public object? Value { get; set; }
    }
}