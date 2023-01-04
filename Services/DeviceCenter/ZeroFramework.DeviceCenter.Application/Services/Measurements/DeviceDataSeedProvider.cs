using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Measurements
{
    public class DeviceDataSeedProvider : IDataSeedProvider
    {
        private readonly IRepository<Product, Guid> _productRepository;

        private readonly IRepository<Device, long> _deviceRepository;

        private readonly IDeviceDataApplicationService _deviceApplicationService;

        public DeviceDataSeedProvider(IRepository<Product, Guid> productRepository, IRepository<Device, long> deviceRepository, IDeviceDataApplicationService deviceDataApplicationService)
        {
            _productRepository = productRepository;
            _deviceRepository = deviceRepository;
            _deviceApplicationService = deviceDataApplicationService;
        }

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await CreateDevicesAsync();

            List<Product> productList = await _productRepository.GetListAsync();

            List<Device> deviceList = await _deviceRepository.GetListAsync();

            Random random = new(Guid.NewGuid().GetHashCode());

            foreach (Device device in deviceList)
            {
                var values = await _deviceApplicationService.GetDevicePropertyValues(device.ProductId, device.Id);

                if (values is not null && values.Max(e => e.Timestamp) > DateTimeOffset.Now.AddDays(-1).ToUnixTimeMilliseconds())
                {
                    continue;
                }

                var properties = productList.SingleOrDefault(e => e.Id == device.ProductId)?.Features?.Properties ?? Enumerable.Empty<PropertyFeature>();

                foreach (var propery in properties)
                {
                    DateTimeOffset startDate = DateTimeOffset.Now.AddDays(-1).Date;

                    while (startDate < DateTimeOffset.Now)
                    {
                        DevicePropertyValue devicePropertyValue = new()
                        {
                            Timestamp = startDate.AddSeconds(random.Next(1, 60)).ToUnixTimeMilliseconds()
                        };

                        if (propery.DataType.Type is DataTypeDefinitions.Int32 or DataTypeDefinitions.Int64)
                        {
                            devicePropertyValue.Value = random.Next(ushort.MaxValue);
                        }

                        if (propery.DataType.Type is DataTypeDefinitions.Float or DataTypeDefinitions.Double)
                        {
                            if (propery.DataType.Specs is not null)
                            {
                                int min = Convert.ToInt32(propery.DataType.Specs.First(e => e.Key == "minValue").Value?.ToString());
                                int max = Convert.ToInt32(propery.DataType.Specs.First(e => e.Key == "maxValue").Value?.ToString());
                                devicePropertyValue.Value = random.Next(min, max) + Random.Shared.NextDouble() * 10;
                            }
                        }

                        if (propery.DataType.Type is DataTypeDefinitions.Bool)
                        {
                            devicePropertyValue.Value = random.Next(0, 10) % 2 == 0;
                        }

                        if (propery.DataType.Type is DataTypeDefinitions.Date)
                        {
                            devicePropertyValue.Value = DateTimeOffset.Now.AddMilliseconds(-random.Next(int.MaxValue)).ToUnixTimeMilliseconds();
                        }

                        if (propery.DataType.Type is DataTypeDefinitions.String)
                        {
                            devicePropertyValue.Value = Path.GetRandomFileName();
                        }

                        if (devicePropertyValue.Value is not null)
                        {
                            await _deviceApplicationService.SetDevicePropertyValue(device.ProductId, device.Id, propery.Identifier, devicePropertyValue);
                        }

                        startDate = startDate.AddMinutes(30);
                    }
                }
            }
        }

        public async Task CreateDevicesAsync()
        {
            if (await _deviceRepository.CountAsync() <= 0)
            {
                var product1 = await _productRepository.SingleOrDefaultAsync(e => e.Name.Contains("环境空气质量监测产品"));

                if (product1 is not null)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        var device = new Device
                        {
                            Name = $"环境空气监测站{i + 1}",
                            Coordinate = new GeoCoordinate(Random.Shared.Next(75, 118), Random.Shared.Next(23, 41)),
                            CreationTime = DateTimeOffset.Now,
                            LastOnlineTime = DateTimeOffset.Now,
                            Status = (DeviceStatus)Random.Shared.Next(0, 3),
                            ProductId = product1.Id
                        };

                        await _deviceRepository.InsertAsync(device, true);
                    }
                }

                var product2 = await _productRepository.SingleOrDefaultAsync(e => e.Name.Contains("水质监测产品"));

                if (product2 is not null)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var device = new Device
                        {
                            Name = $"水质监测设备{i + 1}",
                            Coordinate = new GeoCoordinate(Random.Shared.Next(75, 118), Random.Shared.Next(23, 41)),
                            CreationTime = DateTimeOffset.Now,
                            LastOnlineTime = DateTimeOffset.Now,
                            Status = (DeviceStatus)Random.Shared.Next(0, 3),
                            ProductId = product2.Id
                        };

                        await _deviceRepository.InsertAsync(device, true);
                    }
                }

                var product3 = await _productRepository.SingleOrDefaultAsync(e => e.Name.Contains("流量液位压力监测产品"));

                if (product3 is not null)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var device = new Device
                        {
                            Name = $"流量液位压力仪{i + 1}",
                            Coordinate = new GeoCoordinate(Random.Shared.Next(75, 118), Random.Shared.Next(23, 41)),
                            CreationTime = DateTimeOffset.Now,
                            LastOnlineTime = DateTimeOffset.Now,
                            Status = (DeviceStatus)Random.Shared.Next(0, 3),
                            ProductId = product3.Id
                        };

                        await _deviceRepository.InsertAsync(device, true);
                    }
                }

                var product4 = await _productRepository.SingleOrDefaultAsync(e => e.Name.Contains("气象土壤监测产品"));

                if (product4 is not null)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var device = new Device
                        {
                            Name = $"气象土壤监测站{i + 1}",
                            Coordinate = new GeoCoordinate(Random.Shared.Next(75, 118), Random.Shared.Next(23, 41)),
                            CreationTime = DateTimeOffset.Now,
                            LastOnlineTime = DateTimeOffset.Now,
                            Status = (DeviceStatus)Random.Shared.Next(0, 3),
                            ProductId = product4.Id
                        };

                        await _deviceRepository.InsertAsync(device, true);
                    }
                }
            }
        }
    }
}
