using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Monitoring;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MonitoringAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class MonitoringProfile : Profile
    {
        public MonitoringProfile()
        {
            CreateMap<MonitoringFactor, MonitoringFactorGetResponseModel>();
            CreateMap<MonitoringFactorCreateRequestModel, MonitoringFactor>();
            CreateMap<MonitoringFactorUpdateRequestModel, MonitoringFactor>();
        }
    }
}
