using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Application.Models.Measurements
{
    public class DevicePropertyLastValue : DevicePropertyValue
    {
        [AllowNull]
        public string Identifier { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public string? Unit { get; set; }
    }
}
