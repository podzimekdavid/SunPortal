using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Service.Database.Services;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DevicesController : Controller
{
    private readonly Lib.Interfaces.IDevicesService _devicesesService;

    public DevicesController(Lib.Interfaces.IDevicesService devicesesService)
    {
        _devicesesService = devicesesService;
    }

    [HttpGet(Lib.Communication.Endpoints.CLIENTS)]
    public IEnumerable<Client>? Clients(string ownerId)
    {
        return _devicesesService.ClientsByOwner(ownerId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.PARAMETERS)]
    public IEnumerable<Lib.App.DeviceParameter>? Parameters(Guid deviceId, ParameterPriority? priority = null)
    {
        return priority.HasValue
            ? _devicesesService.ParametersByDevice(deviceId, priority.Value).Result
            : _devicesesService.ParametersByDevice(deviceId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.DEVICES)]
    public IEnumerable<Lib.App.Device>? Devices(Guid clientId)
    {
        return _devicesesService.DevicesByClient(clientId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.CLIENT)]
    public Lib.App.Client? Client(Guid clientId)
    {
        return _devicesesService.Client(clientId).Result;
    }
    
    [HttpGet(Lib.Communication.Endpoints.DEVICE)]
    public Lib.App.Device? Device(Guid deviceId)
    {
        return _devicesesService.Device(deviceId).Result;
    }
}