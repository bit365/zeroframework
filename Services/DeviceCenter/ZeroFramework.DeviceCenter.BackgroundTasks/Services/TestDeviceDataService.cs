
//namespace ZeroFramework.DeviceCenter.BackgroundTasks.Services
//{
//    public class TestDeviceDataService
//    {
//        private readonly ILogger<TestDeviceDataService> _logger;

//        private readonly HttpClient _httpClient;

//        private readonly IConfiguration _configuration;

//        private readonly IDistributedCache _cache;

//        public TestDeviceDataService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<TestDeviceDataService> logger, IDistributedCache cache)
//        {
//            _httpClient = httpClientFactory.CreateClient();
//            _configuration = configuration;
//            _logger = logger;
//            _cache = cache;
//        }

//        public async Task<string?> GetAccessTokenAsync()
//        {
//            string? accessToken = await _cache.GetStringAsync(nameof(TokenResponse.AccessToken));

//            if (accessToken == null)
//            {
//                string? identityEndpoint = _configuration.GetRequiredSection("IdentityServer:AuthorizationUrl").Value;

//                var disco = await _httpClient.GetDiscoveryDocumentAsync(identityEndpoint);

//                var tokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
//                {
//                    Address = disco.TokenEndpoint,

//                    ClientId = "devicecenterswagger",
//                    ClientSecret = "secret",

//                    UserName = "admin",
//                    Password = "guest",

//                    Scope = "openid devicecenter"
//                });

//                if (!tokenResponse.IsError)
//                {
//                    accessToken = tokenResponse.AccessToken;

//                    await _cache.SetStringAsync(nameof(TokenResponse.AccessToken), accessToken, new DistributedCacheEntryOptions
//                    {
//                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
//                    });
//                }
//                else
//                {
//                    _logger.LogError(tokenResponse.Error);
//                }
//            }

//            return accessToken;
//        }

//        public async Task GetProductInfo()
//        {
//            string deviceEndpoint = _configuration.GetRequiredSection("DeviceCenterEndPoint").Value!;

//            string? accessToken = await GetAccessTokenAsync();

//            if (accessToken is not null)
//            {
//                _httpClient.SetBearerToken(accessToken);
//            }

//            _httpClient.BaseAddress = new Uri(deviceEndpoint);

//            var response = await _httpClient.GetAsync("api/Products");

//            try
//            {
//                response.EnsureSuccessStatusCode();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Device Center API Response Error.");
//                return;
//            }

//            var content = await response.Content.ReadAsStringAsync();
//        }
//    }
//}
