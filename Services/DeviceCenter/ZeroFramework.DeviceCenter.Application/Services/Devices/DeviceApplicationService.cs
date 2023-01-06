using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Devices;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public class DeviceApplicationService : CrudApplicationService<Device, long, DeviceGetResponseModel, DevicePagedRequestModel, DeviceGetResponseModel, DeviceCreateRequestModel, DeviceUpdateRequestModel>, IDeviceApplicationService
    {
        private readonly IRepository<Product, int> _productRepository;

        private readonly IMapper _mapper;

        private readonly IDeviceRepository _deviceRepository;

        public DeviceApplicationService(IDeviceRepository deviceRepository, IRepository<Product, int> productRepository, IMapper mapper) : base(deviceRepository, mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
        }

        public override async Task<DeviceGetResponseModel> GetAsync(long id)
        {
            DeviceGetResponseModel deviceGetResponseModel = await base.GetAsync(id);
            Product product = await _productRepository.GetAsync(deviceGetResponseModel.ProductId);
            deviceGetResponseModel.Product = _mapper.Map<ProductGetResponseModel>(product);
            return deviceGetResponseModel;
        }

        public override async Task<PagedResponseModel<DeviceGetResponseModel>> GetListAsync(DevicePagedRequestModel requestModel)
        {
            var entities = await _deviceRepository.GetListAsync(requestModel.ProductId, requestModel.DeviceGroupId, requestModel.Status, requestModel.Name, requestModel.PageNumber, requestModel.PageSize);

            int totalCount = await _deviceRepository.GetCountAsync(requestModel.ProductId, requestModel.DeviceGroupId, requestModel.Status, requestModel.Name);

            var entityDtos = _mapper.Map<List<DeviceGetResponseModel>>(entities);

            IEnumerable<int> productIds = entityDtos.Select(e => e.ProductId).Distinct();

            List<Product> products = await _productRepository.AsyncExecuter.ToListAsync(_productRepository.Query.Where(e => productIds.Contains(e.Id)));

            IEnumerable<ProductGetResponseModel> productModels = _mapper.Map<IEnumerable<ProductGetResponseModel>>(products);

            foreach (var item in entityDtos)
            {
                item.Product = productModels.First(e => e.Id == item.ProductId);
            }

            return new PagedResponseModel<DeviceGetResponseModel>(entityDtos, totalCount);
        }

        public async Task<DeviceStatisticGetResponseModel> GetStatistics()
        {
            DeviceStatisticGetResponseModel deviceStatistic = new()
            {
                TotalCount = await Repository.CountAsync(),
                OfflineCount = await Repository.CountAsync(e => e.Status == DeviceStatus.Offline),
                OnlineCount = await Repository.CountAsync(e => e.Status == DeviceStatus.Online),
                UnactiveCount = await Repository.CountAsync(e => e.Status == DeviceStatus.Unactive)
            };

            return deviceStatistic;
        }
    }
}