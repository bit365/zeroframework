namespace ZeroFramework.DeviceCenter.Application.Models.Devices
{
    public class DeviceStatisticGetResponseModel
    {
        public int TotalCount { get; set; }

        public int OnlineCount { get; set; }

        public int OfflineCount { get; set; }

        public int UnactiveCount { get; set; }
    }
}
