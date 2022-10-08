using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Infrastructure.ConnectionStrings
{
    [Serializable]
    public class TenantConfiguration
    {
        public Guid TenantId { get; set; }

        [AllowNull]
        public string TenantName { get; set; }

        public Dictionary<string, string>? ConnectionStrings { get; set; }
    }
}