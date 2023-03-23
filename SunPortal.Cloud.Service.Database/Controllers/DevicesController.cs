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

    public IEnumerable<Lib.App.Client> Clients(string ownerId)
    {
        return _devicesService.ClientsByOwner(ownerId);
    }

    public IEnumerable<Lib.App.DeviceParameter> Parameters(Guid deviceId, ParameterPriority? priority = null)
    {
        return priority.HasValue ? 
            _devicesService.ParametersByDevice(deviceId, priority.Value) : 
            _devicesService.ParametersByDevice(deviceId);
    }
}