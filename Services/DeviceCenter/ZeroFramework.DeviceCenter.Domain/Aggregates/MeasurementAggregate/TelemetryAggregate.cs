namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class TelemetryAggregate
    {
        public string Time { get; set; } = default!;

        public double? Min { get; set; }

        public double? Average { get; set; }

        public double? Max { get; set; }

        public int Count { get; set; }
    }
}
