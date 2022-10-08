using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ZeroFramework.IdentityServer.API.Infrastructure.Aliyun
{
    /// <summary>
    /// The Alibaba Cloud SDK for .NET allows you to access Alibaba Cloud services such as Elastic Compute Service (ECS), Server Load Balancer (SLB), CloudMonitor, etc.
    /// You can access Alibaba Cloud services without the need to handle API related tasks, such as signing and constructing your requests.
    /// https://github.com/aliyun/aliyun-openapi-net-sdk
    /// </summary>
    public class AliyunAuthHandler : DelegatingHandler
    {
        private readonly AlibabaCloudOptions _alibabaCloudOptions;

        public AliyunAuthHandler(IOptions<AlibabaCloudOptions> alibabaCloudOptionsAccessor)
        {
            _alibabaCloudOptions = alibabaCloudOptionsAccessor.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Dictionary<string, string?> parameters = new()
            {
                { "SignatureMethod", "HMAC-SHA1" },
                { "SignatureNonce", Guid.NewGuid().ToString() },
                { "SignatureVersion", "1.0" },
                { "AccessKeyId", _alibabaCloudOptions.AccessKeyId },
                { "Timestamp", DateTime.Now.ToUniversalTime().ToString("o") },
                { "Format", "JSON" }
            };

            foreach (var property in request.Options)
            {
                if (property.Value is not null)
                {
                    var type = property.Value.GetType();
                    bool isPrimitiveType = type.IsPrimitive || type.IsValueType || (type == typeof(string));
                    string? propertyValue = isPrimitiveType ? property.Value.ToString() : System.Text.Json.JsonSerializer.Serialize(property.Value);
                    parameters[property.Key] = propertyValue;
                }
            }

            var sortedDictionary = new SortedDictionary<string, string?>(parameters, StringComparer.Ordinal);

            StringBuilder canonicalizedQueryString = new();

            foreach (var p in sortedDictionary)
            {
                if (p.Value is not null)
                {
                    canonicalizedQueryString.Append('&').Append(PercentEncode(p.Key)).Append('=').Append(PercentEncode(p.Value));
                }
            }

            StringBuilder stringToSign = new();

            stringToSign.Append(request.Method).Append('&').Append(PercentEncode("/")).Append('&');
            stringToSign.Append(PercentEncode(canonicalizedQueryString.ToString()[1..]));

            string signature = string.Empty;

            using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(_alibabaCloudOptions.AccessKeySecret + "&")))
            {
                var hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign.ToString()));
                signature = Convert.ToBase64String(hashValue);
            }

            signature = PercentEncode(signature);

            request.RequestUri = new Uri($"{request.RequestUri?.Scheme}://{request.RequestUri?.Host}/?Signature={signature}{canonicalizedQueryString}");

            return await base.SendAsync(request, cancellationToken);
        }

        private static string PercentEncode(string value)
        {
            StringBuilder stringBuilder = new();

            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            foreach (char c in bytes)
            {
                if (text.Contains(c))
                {
                    stringBuilder.Append(c);
                }
                else
                {
                    stringBuilder.Append('%').Append(string.Format(CultureInfo.InvariantCulture, "{0:X2}", (int)c));
                }
            }
            return stringBuilder.ToString();
        }
    }
}
