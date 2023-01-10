using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using static ZeroFramework.DeviceCenter.Application.PermissionProviders.DeviceGroupPermissions;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.YuanGan
{
    public class YuanGanWorker : BackgroundService
    {
        private readonly IRepository<Product, int> _productRepository;

        private readonly IRepository<Device, long> _deviceRepository;

        private readonly IDeviceDataApplicationService _deviceApplicationService;

        private readonly ILogger<YuanGanWorker> _logger;

        private readonly HttpClient _httpClient;

        private readonly IRepository<DeviceGroup, int> _deviceGroupRepository;

        private readonly IRepository<DeviceGrouping> _deviceGroupingRepository;

        public YuanGanWorker(IRepository<Product, int> productRepository, IRepository<Device, long> deviceRepository, IDeviceDataApplicationService deviceDataApplicationService, ILogger<YuanGanWorker> logger, IHttpClientFactory httpClientFactory, IRepository<DeviceGroup, int> deviceGroupRepository, IRepository<DeviceGrouping> deviceGroupingRepository)
        {
            _productRepository = productRepository;
            _deviceRepository = deviceRepository;
            _deviceApplicationService = deviceDataApplicationService;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _deviceGroupRepository = deviceGroupRepository;
            _deviceGroupingRepository = deviceGroupingRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    await StartPollingAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Polling device data error");
                }

                await Task.Delay(TimeSpan.FromMinutes(8), stoppingToken);
            }
        }

        private async Task StartPollingAsync(CancellationToken stoppingToken)
        {
            string waterCloudApi = "https://www.waterqualitycloud.com/api/detaildata?userName=cqsyf&password=cqsyf322012&format=json";

            var response = await _httpClient.GetAsync(waterCloudApi, stoppingToken);

            var monitorModels = await response.Content.ReadFromJsonAsync<List<MonitorModel>>(cancellationToken: stoppingToken) ?? Enumerable.Empty<MonitorModel>();

            Product product = await _productRepository.GetAsync(e => e.Name == "多参数水质分析仪", cancellationToken: stoppingToken);

            var properties = product.Features?.Properties?.ToDictionary(p => Convert.ToInt32(p.DataType?.Specs?.First(e => e.Key == "sensorId").Value?.ToString()), p => p);

            foreach (var monitorModel in monitorModels)
            {
                var parentGroup = await _deviceGroupRepository.FindAsync(e => e.Name == monitorModel.ProvinceName, true, stoppingToken);

                if (parentGroup is null)
                {
                    parentGroup = new DeviceGroup()
                    {
                        Name = monitorModel.ProvinceName
                    };

                    parentGroup = await _deviceGroupRepository.InsertAsync(parentGroup, true, stoppingToken);
                }

                var childGroup = await _deviceGroupRepository.FindAsync(e => e.Name == monitorModel.CityName, true, stoppingToken);

                if (childGroup is null)
                {
                    childGroup = new DeviceGroup()
                    {
                        Name = monitorModel.CityName,
                        ParentId = parentGroup.Id,
                    };

                    parentGroup = await _deviceGroupRepository.InsertAsync(childGroup, true, stoppingToken);
                }

                var device = await _deviceRepository.FindAsync(e => e.Remark == monitorModel.Id.ToString(), true, stoppingToken);

                if (device == null)
                {
                    device = new Device
                    {
                        Name = monitorModel.Name,
                        Coordinate = (GeoCoordinate?)monitorModel.Location,
                        CreationTime = DateTimeOffset.Now,
                        Remark = monitorModel.Id.ToString(),
                        ProductId = product.Id,
                        Status = monitorModel.IsOnline ? DeviceStatus.Online : DeviceStatus.Offline,
                    };

                    device = await _deviceRepository.InsertAsync(device, true, stoppingToken);

                    await _deviceGroupingRepository.InsertAsync(new DeviceGrouping { DeviceId = device.Id, DeviceGroupId = childGroup.Id }, true, stoppingToken);
                }
                else
                {
                    device.Coordinate = (GeoCoordinate?)monitorModel.Location;
                    device.Status = monitorModel.IsOnline ? DeviceStatus.Online : DeviceStatus.Offline;
                    device.Name = monitorModel.Name;

                    await _deviceRepository.UpdateAsync(device, true, stoppingToken);
                }

                var propertyValues = new Dictionary<string, DevicePropertyValue>();

                foreach (var sensorValueModel in monitorModel.SensorValues)
                {
                    DevicePropertyValue devicePropertyValue = new()
                    {
                        Timestamp = sensorValueModel.CreateDate.ToUnixTimeMilliseconds(),
                        Value = sensorValueModel.Value,
                    };

                    if (properties != null && properties.TryGetValue(sensorValueModel.SensorID, out PropertyFeature? property))
                    {
                        propertyValues.Add(property.Identifier, devicePropertyValue);
                    }
                }

                if (propertyValues.Any())
                {
                    _logger.LogInformation("productId={productId},deviceId={deviceId},values={values}", device.ProductId, device.Id, propertyValues);

                    //await _deviceApplicationService.SetDevicePropertyValues(device.ProductId, device.Id, propertyValues);
                }
            }
        }
    }
}