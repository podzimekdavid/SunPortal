using SunPortal.Cloud.Lib;
using SunPortal.Cloud.Lib.UI;

namespace SunPortal.Cloud.Services.Portal.Services;

public class DataService
{
    private readonly HttpClient _client;

    public DataService()
    {
        _client = new();
        _client.BaseAddress = new(Lib.Communication.Endpoints.BASE);
    }

    public async Task<IEnumerable<ChartTimeValue>?> FloatValues(Guid deviceId, int parameterId, DateTimeOffset start,
        DateTimeOffset end)
    {
        return await _client.GetFromJsonAsync<IEnumerable<ChartTimeValue>?>
        ($"{Communication.Endpoints.LOG_FLOAT_VALUES}?deviceId={deviceId}" +
         $"&parameterId={parameterId}" +
         $"&start={start.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}" +
         $"&end={end.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}");
    }
}