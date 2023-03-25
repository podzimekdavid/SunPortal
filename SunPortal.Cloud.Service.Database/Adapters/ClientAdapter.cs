using Mapster;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Service.Database.Data;

namespace SunPortal.Cloud.Service.Database.Adapters;

public static class ClientAdapter
{
    public static TypeAdapterConfig DTOConfig;

    static ClientAdapter()
    {
        DTOConfig = new TypeAdapterConfig()
            .ForType<Database.Data.Client, Lib.App.Client>()
            .Map(dst => dst.DevicesCount, src => src.Devices.Count()).Config;
    }
}