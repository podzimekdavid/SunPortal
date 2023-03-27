using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Lib.UI;
using SunPortal.Cloud.Service.Database.Services;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DataController : Controller
{
    private readonly DataService _dataService;


    public DataController(DataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet(Lib.Communication.Endpoints.LOG_FLOAT_VALUES)]
    public IEnumerable<ChartTimeValue>? FloatValues(Guid deviceId, int parameterId, DateTimeOffset start,
        DateTimeOffset end)
    {
        return _dataService.FloatValues(deviceId, parameterId, start, end).Result;
    }
}