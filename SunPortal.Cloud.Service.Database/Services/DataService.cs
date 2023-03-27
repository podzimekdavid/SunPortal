using Mapster;
using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.UI;
using SunPortal.Cloud.Service.Database.Adapters;
using SunPortal.Cloud.Service.Database.Data;

namespace SunPortal.Cloud.Service.Database.Services;

public class DataService
{
    private readonly DatabaseContext _database;

    public DataService(DatabaseContext database)
    {
        _database = database;
    }

    public Task<IEnumerable<ChartTimeValue>?> FloatValues(Guid deviceId, int parameterId, DateTimeOffset start,
        DateTimeOffset end)
    {
        if (!_database.Parameters.Any(x => x.ParameterId == parameterId))
            return Task.FromResult<IEnumerable<ChartTimeValue>?>(null);

        return Task.FromResult<IEnumerable<ChartTimeValue>?>(_database.Logs
            .Where(x => x.ClientDeviceId == deviceId && x.ParameterId == parameterId)
            .Where(x => x.DateTime - start >= TimeSpan.Zero && x.DateTime - end <= TimeSpan.Zero)
            .ProjectToType<ChartTimeValue>(ChartTimeValueAdapter.DTOConfig).ToList());
    }
    
    
}