using ZeroFramework.DeviceCenter.Application.Models.Projects;
using ZeroFramework.DeviceCenter.Application.Services.Generics;

namespace ZeroFramework.DeviceCenter.Application.Services.Projects
{
    public interface IProjectApplicationService
    {
        Task<ProjectGetResponseModel> CreateAsync(ProjectCreateOrUpdateRequestModel requestModel);

        Task DeleteAsync(int id);

        Task<ProjectGetResponseModel> UpdateAsync(int id, ProjectCreateOrUpdateRequestModel requestModel);

        Task<ProjectGetResponseModel> GetAsync(int id);

        Task<PagedResponseModel<ProjectGetResponseModel>> GetListAsync(PagedRequestModel requestModel);
    }
}