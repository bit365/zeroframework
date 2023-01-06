using System.Text.Json.Serialization;
using ZeroFramework.DeviceCenter.Application.Infrastructure;

namespace ZeroFramework.DeviceCenter.Application.Models.Measurements
{
    public class DevicePropertyValue
    {
        public long? Timestamp { get; set; } = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        [JsonConverter(typeof(ObjectToInferredTypesConverter))]
        public object? Value { get; set; }
    }
}