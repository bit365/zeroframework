namespace ZeroFramework.DeviceCenter.BackgroundTasks.YuanGan
{
    public class SensorValueModel
    {
        public int SensorID { get; set; }

        public string SensorName { get; set; } = default!;

        public string? DisplayName { get; set; }

        public float Value { get; set; }

        public string? SensorUnit { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool IsWarn { get; set; }

        public double? LowerValue { get; set; }

        public double? UpperValue { get; set; }
    }
}
