namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class DeviceTelemetry
    {
        public int DeviceId { get; set; }

        public List<TelemetryValue> Values { get; set; } = new();
    }
}
