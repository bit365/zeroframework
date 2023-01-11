using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;
using ZeroFramework.DeviceCenter.BackgroundTasks.YuanGan;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.HuanJing212
{
    public class DictionaryDataService : IDictionaryDataService
    {
        private readonly IRepository<Product, int> _productRepository;

        private readonly IRepository<Device, long> _deviceRepository;

        private readonly IDeviceDataApplicationService _deviceApplicationService;

        private readonly ILogger<DictionaryDataService> _logger;

        public DictionaryDataService(IRepository<Product, int> productRepository, IRepository<Device, long> deviceRepository, IDeviceDataApplicationService deviceApplicationService, ILogger<DictionaryDataService> logger)
        {
            _productRepository = productRepository;
            _deviceRepository = deviceRepository;
            _deviceApplicationService = deviceApplicationService;
            _logger = logger;
        }

        public async Task HandlingAaync(Dictionary<string, string> keyValuePairs)
        {
            Product product = await _productRepository.GetAsync(e => e.Name == "李河污水监测站");

            var properties = product.Features?.Properties?.ToDictionary(p =>p.DataType?.Specs?.FirstOrDefault(e => e.Key == "externalId").Value?.ToString()??string.Empty, p => p);

            var device = await _deviceRepository.FindAsync(e => e.Remark == keyValuePairs["MN"], true);

            if (device == null)
            {
                device = new Device
                {
                    Name = $"李河监测{keyValuePairs["MN"]}",
                    Coordinate = (GeoCoordinate?)"108.272613,30.82183",
                    CreationTime = DateTimeOffset.Now,
                    Remark = keyValuePairs["MN"],
                    ProductId = product.Id,
                    Status =  DeviceStatus.Online
                };

                device = await _deviceRepository.InsertAsync(device, true);
            }

            var propertyValues = new Dictionary<string, DevicePropertyValue>();

            DateTimeOffset dateTime = DateTime.ParseExact(keyValuePairs["DataTime"],"yyyyMMddHHmmss", CultureInfo.CurrentCulture);

            var sensorValues = keyValuePairs.Where(x=>x.Key.EndsWith("Rtd"));

            foreach (var sensorValue in sensorValues)
            {
                DevicePropertyValue devicePropertyValue = new()
                {
                    Timestamp = dateTime.ToUnixTimeMilliseconds(),
                    Value = Convert.ToDouble(sensorValue.Value),
                };

                if (properties != null && properties.TryGetValue(sensorValue.Key.TrimEnd("-Rtd".ToArray()), out PropertyFeature? property))
                {
                    propertyValues.Add(property.Identifier, devicePropertyValue);
                }
            }

            if (propertyValues.Any())
            {
                _logger.LogInformation("productId={productId},deviceId={deviceId},values={values}", device.ProductId, device.Id, propertyValues);

                await _deviceApplicationService.SetDevicePropertyValues(device.ProductId, device.Id, propertyValues);
            }
        }
    }
}
