using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Tenants;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.TenantAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;
using ZeroFramework.DeviceCenter.Infrastructure.Constants;

namespace ZeroFramework.DeviceCenter.Application.Services.Tenants
{
    public class TenantApplicationService(IRepository<Tenant, Guid> tenantRepository, IMapper mapper) : ITenantApplicationService
    {
        protected readonly IRepository<Tenant, Guid> _tenantRepository = tenantRepository;

        private readonly IMapper _mapper = mapper;

        public async Task<TenantGetResponseModel> CreateAsync(TenantCreateOrUpdateRequestModel requestModel)
        {
            Tenant tenant = _mapper.Map<Tenant>(requestModel);
            return _mapper.Map<TenantGetResponseModel>(await _tenantRepository.InsertAsync(tenant, true));
        }

        public async Task DeleteAsync(Guid id) => await _tenantRepository.DeleteAsync(id);

        public async Task DeleteDefaultConnectionStringAsync(Guid id)
        {
            Tenant tenant = await _tenantRepository.GetAsync(id);
            tenant.ConnectionStrings.RemoveAll(c => c.Name == DbConstants.DefaultConnectionStringName);
            await _tenantRepository.UpdateAsync(tenant);
        }

        public async Task<TenantGetResponseModel> GetAsync(Guid id) => _mapper.Map<TenantGetResponseModel>(await _tenantRepository.GetAsync(id));

        public async Task<string?> GetDefaultConnectionStringAsync(Guid id)
        {
            Tenant tenant = await _tenantRepository.GetAsync(id);
            return tenant?.ConnectionStrings?.FirstOrDefault(c => c.Name == DbConstants.DefaultConnectionStringName)?.Value;
        }

        public async Task<PagedResponseModel<TenantGetResponseModel>> GetListAsync(PagedRequestModel requestModel)
        {
            long count = await _tenantRepository.GetCountAsync();
            List<Tenant> list = await _tenantRepository.GetListAsync(requestModel.PageNumber, requestModel.PageSize, t => t.Id);

            return new PagedResponseModel<TenantGetResponseModel>(_mapper.Map<List<TenantGetResponseModel>>(list), (int)count);
        }

        public async Task<TenantGetResponseModel> UpdateAsync(Guid id, TenantCreateOrUpdateRequestModel requestModel)
        {
            Tenant tenant = _mapper.Map<Tenant>(requestModel);
            return _mapper.Map<TenantGetResponseModel>(await _tenantRepository.UpdateAsync(tenant));
        }

        public async Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
        {
            Tenant tenant = await _tenantRepository.GetAsync(id);

            TenantConnectionString? connectionString = tenant.ConnectionStrings.FirstOrDefault(c => c.Name == DbConstants.DefaultConnectionStringName);

            if (connectionString is not null)
            {
                connectionString.Value = defaultConnectionString;
            }
            else
            {
                connectionString = new TenantConnectionString
                {
                    TenantId = tenant.Id,
                    Name = DbConstants.DefaultConnectionStringName,
                    Value = defaultConnectionString
                };
                tenant.ConnectionStrings.Add(connectionString);
            }

            await _tenantRepository.UpdateAsync(tenant);
        }
    }
}