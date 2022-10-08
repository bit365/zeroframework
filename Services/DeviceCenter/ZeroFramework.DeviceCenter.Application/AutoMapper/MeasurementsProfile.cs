using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Measurements;
using ZeroFramework.DeviceCenter.Domain.Aggregates.MeasurementAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class MeasurementsProfile : Profile
    {
        public MeasurementsProfile()
        {
            CreateMap<TelemetryAggregate, DevicePropertyReport>();
        }
    }
}
