using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Monitoring
{
    public class MonitoringFactorUpdateRequestModel
    {
        public int Id { get; set; }

        [AllowNull]
        public string FactorCode { get; set; }

        [AllowNull]
        public string ChineseName { get; set; }

        [AllowNull]
        public string EnglishName { get; set; }

        public string? Unit { get; set; }

        public int Decimals { get; set; }

        public string? Remarks { get; set; }
    }
}
