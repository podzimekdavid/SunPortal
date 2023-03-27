using Mapster;
using SunPortal.Cloud.Lib.App;

namespace SunPortal.Cloud.Service.Database.Adapters;

public class ChartAdapter
{
    public static TypeAdapterConfig DTOConfig;

    static ChartAdapter()
    {
        DTOConfig = new TypeAdapterConfig()
            .ForType<Database.Data.GroupChart, Lib.App.Chart>()
            .Map(dst => dst.PrimaryYParameter, src => src.PrimaryYParameter.Adapt<DeviceParameter>())
            .Map(dst => dst.SecondaryYParameter, src => src.SecondaryYParameter.Adapt<DeviceParameter?>())
            .Config;
    }
}