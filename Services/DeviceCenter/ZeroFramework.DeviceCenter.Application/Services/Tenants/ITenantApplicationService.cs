using ZeroFramework.DeviceCenter.Application.Models.Tenants;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Tenants
{
    public interface ITenantApplicationService : ICrudApplicationService<Guid, TenantGetResponseModel, PagedRequestModel, TenantGetResponseModel, TenantCreateOrUpdateRequestModel, TenantCreateOrUpdateRequestModel>
    {
        Task<string?> GetDefaultConnectionStringAsync(Guid id);

        Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString);

        Task DeleteDefaultConnectionStringAsync(Guid id);
    }
}