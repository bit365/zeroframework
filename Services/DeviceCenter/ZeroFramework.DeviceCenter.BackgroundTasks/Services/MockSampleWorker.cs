using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Application.Services.Measurements;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.BackgroundTasks.Services
{
    public class MockSampleWorker(IRepository<Product, int> productRepository, IRepository<Device, long> deviceRepository, IDeviceDataApplicationService deviceDataApplicationService, ILogger<MockSampleWorker> logger) : BackgroundService
    {
        private readonly IRepository<Product, int> _productRepository = productRepository;

        private readonly IRepository<Device, long> _deviceRepository = deviceRepository;

        private readonly IDeviceDataApplicationService _deviceApplicationService = deviceDataApplicationService;

        private readonly ILogger<MockSampleWorker> _logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                try
                {
                    await GenerateDeviceMockDataAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Generate device mock data error");
                }

                await Task.Delay(TimeSpan.FromMinutes(8), stoppingToken);
            }
        }

        private async Task GenerateDeviceMockDataAsync(CancellationToken stoppingToken)
        {
            List<Product> productList = await _productRepository.GetListAsync(cancellationToken: stoppingToken);

            List<Device> deviceList = await _deviceRepository.GetListAsync(cancellationToken: stoppingToken);

            Random random = new(Guid.NewGuid().GetHashCode());

            _logger.LogInformation("Start generating mock data...");

            foreach (Device device in deviceList)
            {
                var properties = productList.SingleOrDefault(e => e.Id == device.ProductId)?.Features?.Properties ?? Enumerable.Empty<PropertyFeature>();

                foreach (var propery in properties)
                {
                    DateTimeOffset startDate = DateTimeOffset.Now;

                    DevicePropertyValue devicePropertyValue = new()
                    {
                        Timestamp = startDate.ToUnixTimeMilliseconds()
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
                        await _deviceApplicationService.SetDevicePropertyValues(device.ProductId, device.Id, new Dictionary<string, DevicePropertyValue>
                        {
                            {propery.Identifier, devicePropertyValue }
                        });
                    }
                }
            }

            _logger.LogInformation("All mock data generated.");
        }
    }
}