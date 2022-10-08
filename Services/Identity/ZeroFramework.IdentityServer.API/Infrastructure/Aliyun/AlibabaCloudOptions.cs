using System.Diagnostics.CodeAnalysis;

namespace ZeroFramework.IdentityServer.API.Infrastructure.Aliyun
{
    public class AlibabaCloudOptions
    {
        [AllowNull]
        public string AccessKeyId { get; set; }

        [AllowNull]
        public string AccessKeySecret { get; set; }
    }
}