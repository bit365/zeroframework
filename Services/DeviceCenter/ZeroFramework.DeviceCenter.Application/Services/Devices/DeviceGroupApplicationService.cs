using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Devices;
using ZeroFramework.DeviceCenter.Application.Models.Products;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProductAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Domain.Services.Devices;

namespace ZeroFramework.DeviceCenter.Application.Services.Devices
{
    public class DeviceGroupApplicationService : CrudApplicationService<DeviceGroup, int, DeviceGroupGetResponseModel, DeviceGroupPagedRequestModel, DeviceGroupGetResponseModel, DeviceGroupCreateRequestModel, DeviceGroupUpdateRequestModel>, IDeviceGroupApplicationService
    {
        private readonly IRepository<DeviceGrouping> _deviceGroupingRepository;

        private readonly IRepository<Device, long> _deviceRepository;

        private readonly IRepository<Product, Guid> _productRepository;

        private readonly IMapper _mapper;

        private readonly IDeviceGroupDomainService _deviceGroupDomainService;

        public DeviceGroupApplicationService(IRepository<DeviceGroup, int> deviceGroupRepository, IMapper mapper, IRepository<DeviceGrouping> deviceGroupingRepository, IRepository<Device, long> deviceRepository, IRepository<Product, Guid> productRepository, IDeviceGroupDomainService deviceGroupDomainService) : base(deviceGroupRepository, mapper)
        {
            _deviceGroupingRepository = deviceGroupingRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _productRepository = productRepository;
            _deviceGroupDomainService = deviceGroupDomainService;
        }

        public override async Task<PagedResponseModel<DeviceGroupGetResponseModel>> GetListAsync(DeviceGroupPagedRequestModel requestModel)
        {
            var (items, totalCount) = await _deviceGroupDomainService.GetDeviceGroupListAsync(requestModel.ParentId, requestModel.Keyword, requestModel.PageNumber, requestModel.PageSize);

            var entityDtos = _mapper.Map<List<DeviceGroupGetResponseModel>>(items);

            return new PagedResponseModel<DeviceGroupGetResponseModel>(entityDtos, totalCount);
        }

        public async Task<PagedResponseModel<DeviceGetResponseModel>> GetDeviceListAsync(DevicePagedRequestModel requestModel)
        {
            IQueryable<DeviceGrouping> query = _deviceGroupingRepository.Query;

            query = query.Where(e => e.DeviceGroupId == requestModel.DeviceGroupId);

            if (requestModel.Name is not null && !string.IsNullOrWhiteSpace(requestModel.Name))
            {
                query = query.Where(e => e.Device.Name.Contains(requestModel.Name));
            }

            if (requestModel.ProductId.HasValue)
            {
                query = query.Where(e => e.Device.ProductId == requestModel.ProductId.Value);
            }

            if (requestModel.Status.HasValue)
            {
                query = query.Where(e => e.Device.Status == requestModel.Status.Value);
            }

            int totalCount = await _deviceGroupingRepository.AsyncExecuter.CountAsync(query);

            query = query.Skip((requestModel.PageNumber - 1) * requestModel.PageSize).Take(requestModel.PageSize);

            var groupings = await Repository.AsyncExecuter.ToListAsync(query);

            IEnumerable<long> deviceIds = groupings.Select(e => e.DeviceId);

            List<Device> devices = await _deviceRepository.AsyncExecuter.ToListAsync(_deviceRepository.Query.Where(e => deviceIds.Contains(e.Id)));

            var entityDtos = _mapper.Map<List<DeviceGetResponseModel>>(devices);

            IEnumerable<Guid> productIds = entityDtos.Select(e => e.ProductId).Distinct();

            List<Product> products = await _productRepository.AsyncExecuter.ToListAsync(_productRepository.Query.Where(e => productIds.Contains(e.Id)));

            IEnumerable<ProductGetResponseModel> productModels = _mapper.Map<IEnumerable<ProductGetResponseModel>>(products);

            foreach (var item in entityDtos)
            {
                item.Product = productModels.First(e => e.Id == item.ProductId);
            }

            return new PagedResponseModel<DeviceGetResponseModel>(entityDtos, totalCount);
        }

        public async Task AddDevicesToGroup(int deviceGroupId, params long[] deviceIds)
        {
            var deviceGroupings = _deviceGroupingRepository.Query.Where(e => e.DeviceGroupId == deviceGroupId);

            var newDeviceIds = deviceIds.Where(id => !deviceGroupings.Any(e => e.DeviceId == id));

            var newDeviceGroupings = newDeviceIds.Select(id => new DeviceGrouping { DeviceGroupId = deviceGroupId, DeviceId = id });

            await _deviceGroupingRepository.InsertManyAsync(newDeviceGroupings);

            await _deviceGroupingRepository.UnitOfWork.SaveChangesAsync();
        }

        public async Task RemoveDevicesFromGroup(int deviceGroupId, params long[] deviceIds)
        {
            var deviceGroupings = _deviceGroupingRepository.Query.Where(e => e.DeviceGroupId == deviceGroupId && deviceIds.Contains(e.DeviceId));

            await _deviceGroupingRepository.DeleteManyAsync(deviceGroupings);

            await _deviceGroupingRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}