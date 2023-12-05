using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Projects;
using ZeroFramework.DeviceCenter.Application.Services.Generics;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProjectAggregate;
using ZeroFramework.DeviceCenter.Domain.Repositories;

namespace ZeroFramework.DeviceCenter.Application.Services.Projects
{
    public class ProjectApplicationService(IRepository<Project, int> repository, IMapper mapper) : CrudApplicationService<Project, int, ProjectGetResponseModel, PagedRequestModel, ProjectGetResponseModel, ProjectCreateOrUpdateRequestModel, ProjectCreateOrUpdateRequestModel>(repository, mapper), IProjectApplicationService
    {
    }
}