using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace ZeroFramework.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController(IDistributedCache distributedCache) : ControllerBase
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        const string memberkey = "9VQrbUztIWJGu9IhPPeK";
        const string appkey = "2302600008";
        const string secret = "288Hs33a";
        const string apiEndpoint = "http://219.151.131.31:19103";

        static readonly HttpClient httpClient = new() { BaseAddress = new Uri(apiEndpoint) };

        static readonly Dictionary<string, string> geocodingMap = new()
        {
            { "50000002041320029010", "108.6280,30.6700" }, // 龙驹一级
            { "50000002041320029011", "108.6190,30.6400" }, // 龙驹二级
            { "50000002041320029008", "108.5476,30.7365" }, // 龙滩电站
            { "50000002041320029009", "108.4380,30.7713" }, // 高洞子电站
        };

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            Dictionary<string, object> deviceListparmeters = new()
            {
                { nameof(memberkey), memberkey },
            };

            string deviceListReadAsString = await SendApiRequest(HttpMethod.Post, "api/dict/device/select", deviceListparmeters);

            JsonElement deviceListData = JsonDocument.Parse(deviceListReadAsString).RootElement;

            if (deviceListData.GetProperty("code").GetString() == "0")
            {
                return Ok(deviceListData);
            }

            List<object> result = [];

            foreach (var device in deviceListData.GetProperty("result").EnumerateArray())
            {
                string? deviceid = device.GetProperty("deviceid").GetString() ?? string.Empty;
                string? devicename = device.GetProperty("devicename").GetString();
                int online = device.GetProperty("online").GetInt32();
                string? devicepwd = device.GetProperty("devicepwd").GetString();
                string? address = device.GetProperty("address").GetString();

                Dictionary<string, object> playUriParmeters = new()
                {
                    { nameof(memberkey), memberkey },
                    { nameof(deviceid), deviceid },
                    { "networktype", 1 },
                };

                string? playUriReadAsString = await SendApiRequest(HttpMethod.Post, "/api/dict/media/live", playUriParmeters);
                JsonElement playUriData = JsonDocument.Parse(playUriReadAsString).RootElement;

                string? playUri = null;

                if (playUriData.GetProperty("code").GetString() == "1")
                {
                    playUri = playUriData.GetProperty("result").EnumerateObject().First(e => e.Name == "m3u8uri").Value.GetString();
                }

                string? location = null;

                if (geocodingMap.ContainsKey(deviceid))
                {
                    location = geocodingMap[deviceid];
                }

                result.Add(new { DeivceId = deviceid, DeviceName = devicename, Address = address, PlayUri = playUri, Online = Convert.ToBoolean(online), Location = location });
            }

            return Ok(result);
        }

        static async Task<string> SendApiRequest(HttpMethod httpMethod, string apiName, Dictionary<string, object> parmeters)
        {
            string sign = CreateSign(httpMethod, parmeters, secret);

            Dictionary<string, object> commonParmeters = new()
            {
                { nameof(appkey), appkey },
                { nameof(sign), sign },
            };

            StringBuilder stringBuilder = new();

            foreach (var item in commonParmeters)
            {
                stringBuilder.Append($"{item.Key}={item.Value}&");
            }

            string requestUri = $"{apiName}?{stringBuilder.ToString().TrimEnd('&')}";

            if (httpMethod == HttpMethod.Get)
            {
                foreach (var item in parmeters)
                {
                    stringBuilder.Append($"{item.Key}={item.Value}&");
                }

                requestUri = $"{apiName}?{stringBuilder.ToString().TrimEnd('&')}";

                return await httpClient.GetStringAsync(requestUri);
            }

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            string json = JsonSerializer.Serialize(new { Parmdata = parmeters }, options);
            StringContent content = new(json, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(requestUri, content);

            return await result.Content.ReadAsStringAsync();
        }

        static string CreateSign(HttpMethod httpMethod, Dictionary<string, object> parmeters, string secret)
        {
            string signString = $"{secret}&&";

            SortedDictionary<string, object> sortedParmeters = new(parmeters);

            if (httpMethod == HttpMethod.Get)
            {
                foreach (var item in sortedParmeters)
                {
                    signString += $"{item.Key}={item.Value}&";
                }

                signString = signString.TrimEnd('&');
            }
            else
            {
                signString += JsonSerializer.Serialize(parmeters);
            }

            return ComputeHashMd5(signString);
        }

        public static string ComputeHashMd5(string text)
        {
            using var provider = System.Security.Cryptography.MD5.Create();

            StringBuilder builder = new();

            foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(text)))
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }
    }
}