using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate
{
    public class TelemetryAggregate
    {
        [AllowNull]
        public string Date { get; set; }

        public double? MinValue { get; set; }

        public double? AverageValue { get; set; }

        public double? MaxValue { get; set; }
    }
}
