using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Service.Database.Services;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DevicesController : Controller
{
    private readonly DevicesService _devicesService;

    public DevicesController(DevicesService devicesService)
    {
        _devicesService = devicesService;
    }

    [HttpGet(Lib.Communication.Endpoints.DEVICES)]
    public IEnumerable<Lib.App.Client> Devices(string ownerId)
    {
        return _devicesService.ClientsByOwner(ownerId);
    }

    [HttpGet(Lib.Communication.Endpoints.PARAMETERS)]
    public IEnumerable<Lib.App.DeviceParameter> Parameters(Guid deviceId, ParameterPriority? priority = null)
    {
        return priority.HasValue ? 
            _devicesService.ParametersByDevice(deviceId, priority.Value) : 
            _devicesService.ParametersByDevice(deviceId);
    }
}