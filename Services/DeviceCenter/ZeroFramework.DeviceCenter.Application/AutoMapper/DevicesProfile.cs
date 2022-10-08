using AutoMapper;
using ZeroFramework.DeviceCenter.Application.Models.Devices;
using ZeroFramework.DeviceCenter.Domain.Aggregates.DeviceAggregate;

namespace ZeroFramework.DeviceCenter.Application.AutoMapper
{
    public class DevicesProfile : Profile
    {
        public DevicesProfile()
        {
            AllowNullDestinationValues = true;
            AllowNullCollections = true;

            CreateMap<Device, DeviceGetResponseModel>();
            CreateMap<DeviceCreateRequestModel, Device>();
            CreateMap<DeviceUpdateRequestModel, Device>();

            CreateMap<DeviceGroup, DeviceGroupGetResponseModel>();
            CreateMap<DeviceGroupCreateRequestModel, DeviceGroup>();
            CreateMap<DeviceGroupUpdateRequestModel, DeviceGroup>();
        }
    }
}
