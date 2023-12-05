using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Measurements
{
    public class DeviceDataSeedProvider(IRepository<Product, int> productRepository, IRepository<Device, long> deviceRepository) : IDataSeedProvider
    {
        private readonly IRepository<Product, int> _productRepository = productRepository;

        private readonly IRepository<Device, long> _deviceRepository = deviceRepository;

        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            await CreateDevicesAsync();
        }

        public async Task CreateDevicesAsync()
        {
            if (await _deviceRepository.CountAsync() <= 0)
            {
                var product1 = await _productRepository.SingleOrDefaultAsync(e => e.Name.Contains("环境空气质量监测产品"));

                if (product1 is not null)
                {
                    for (int i = 0; i < 2; i++)
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
                    for (int i = 0; i < 2; i++)
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
                    for (int i = 0; i < 2; i++)
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
                    for (int i = 0; i < 2; i++)
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