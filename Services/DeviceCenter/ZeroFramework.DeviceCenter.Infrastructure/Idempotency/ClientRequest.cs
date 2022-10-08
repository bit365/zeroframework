using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.DeviceCenter.Infrastructure.Idempotency
{
    public class ClientRequest
    {
        [AllowNull]
        public string Id { get; set; }

        [AllowNull]
        public string Name { get; set; }

        public DateTimeOffset Time { get; set; }
    }
}