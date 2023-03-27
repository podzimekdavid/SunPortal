using Microsoft.AspNetCore.Mvc;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Cloud.Lib.Parameters;
using SunPortal.Cloud.Service.Database.Services;

namespace SunPortal.Cloud.Service.Database.Controllers;

public class DevicesController : Controller
{
    private readonly Lib.Interfaces.IDevicesService _devicesService;

    public DevicesController(Lib.Interfaces.IDevicesService devicesService)
    {
        _devicesService = devicesService;
    }

    [HttpGet(Lib.Communication.Endpoints.CLIENTS)]
    public IEnumerable<Client>? Clients(string ownerId)
    {
        return _devicesService.ClientsByOwner(ownerId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.PARAMETERS)]
    public IEnumerable<Lib.App.DeviceParameter>? Parameters(Guid deviceId, ParameterPriority? priority = null)
    {
        return priority.HasValue
            ? _devicesService.ParametersByDevice(deviceId, priority.Value).Result
            : _devicesService.ParametersByDevice(deviceId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.DEVICES)]
    public IEnumerable<Lib.App.Device>? Devices(Guid clientId)
    {
        return _devicesService.DevicesByClient(clientId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.CLIENT)]
    public Lib.App.Client? Client(Guid clientId)
    {
        return _devicesService.Client(clientId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.DEVICE)]
    public Lib.App.Device? Device(Guid deviceId)
    {
        return _devicesService.Device(deviceId).Result;
    }

    [HttpGet(Lib.Communication.Endpoints.SYNC_SETTINGS)]
    public Lib.App.ClientSyncSettings SyncSettings(Guid clientId)
    {
        return _devicesService.SyncSettings(clientId).Result;
    }

    [HttpPost(Lib.Communication.Endpoints.DEVICE_SYNC)]
    public async Task<IActionResult> DeviceSync([FromBody]DeviceSyncPackage package)
    {
        await _devicesService.DeviceSync(package);

        return Ok();
    }
}