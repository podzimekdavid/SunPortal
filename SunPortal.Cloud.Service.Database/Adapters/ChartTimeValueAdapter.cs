using Mapster;

namespace SunPortal.Cloud.Service.Database.Adapters;

public class ChartTimeValueAdapter
{
    public static TypeAdapterConfig DTOConfig;

    static ChartTimeValueAdapter()
    {
        DTOConfig = new TypeAdapterConfig()
            .ForType<Database.Data.DeviceLog, Lib.UI.ChartTimeValue>()
            .Map(dst => dst.Value, src => BitConverter.ToSingle(src.Value))
            .Config;
    }
}