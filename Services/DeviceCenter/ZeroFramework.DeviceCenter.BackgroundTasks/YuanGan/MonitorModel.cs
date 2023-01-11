namespace ZeroFramework.DeviceCenter.BackgroundTasks.YuanGan
{
    public class MonitorModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Location { get; set; } = default!;

        public DateTimeOffset ActiveTime { get; set; }

        public string ProvinceID { get; set; } = default!;

        public string ProvinceName { get; set; } = default!;

        public string CityID { get; set; } = default!;

        public string CityName { get; set; } = default!;

        public bool IsOnline { get; set; }

        public SensorValueModel[] SensorValues { get; set; } = default!;

        public string SerialNumber { get; set; } = default!;

        public string? ExternalSerialNumber { get; set; }
    }
}
