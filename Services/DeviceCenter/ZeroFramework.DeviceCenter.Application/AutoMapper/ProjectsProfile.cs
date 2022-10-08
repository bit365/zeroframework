using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Projects;
using ZeroFramework.DeviceCenter.Domain.Aggregates.ProjectAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class ProjectsProfile : Profile
    {
        public ProjectsProfile()
        {
            CreateMap<Project, ProjectGetResponseModel>();
            CreateMap<ProjectCreateOrUpdateRequestModel, Project>();
        }
    }
}
