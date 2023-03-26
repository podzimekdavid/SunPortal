using Mapster;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Service.Database.Data;

namespace SunPortal.Cloud.Service.Database.Adapters;

public static class DeviceAdapter
{
    public static TypeAdapterConfig DTOConfig;

    static DeviceAdapter()
    {
        DTOConfig = new TypeAdapterConfig()
            .ForType<ClientDevice, Device>()
            .Map(dst => dst.DeviceType, src => src.SupportedDevice.Adapt<DeviceType>()).Config;
    }
}