using SunPortal.Cloud.Service.Database.Data;
using SunPortal.Communication.Parameters;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Interfaces;
using SunPortal.Cloud.Service.Database.Adapters;
using Client = SunPortal.Cloud.Lib.App.Client;

namespace SunPortal.Cloud.Service.Database.Services;

public class DevicesesService : IDevicesService
{
    private readonly DatabaseContext _database;

    public DevicesesService(DatabaseContext database)
    {
        _database = database;
    }

    public Task<IEnumerable<DeviceParameter>?> ParametersByDevice(Guid deviceId, ParameterPriority priority)
    {
        return Task.FromResult<IEnumerable<DeviceParameter>?>(Parameters(deviceId)
            .Where(x => x.Priority == priority).ProjectToType<Lib.App.DeviceParameter>());
    }

    private IQueryable<Parameter> Parameters(Guid deviceId)
    {
        return _database.Parameters.Include(x => x.ParameterGroup)
            .ThenInclude(x => x.SupportedDevices)
            .ThenInclude(x => x.Devices)
            .Where(x => x.ParameterGroup.SupportedDevices
                .Any(x => x.Devices
                    .Any(x => x.ClientDeviceId == deviceId)));
    }


    public Task<IEnumerable<DeviceParameter>?> ParametersByDevice(Guid deviceId)
    {
        return Task.FromResult<IEnumerable<DeviceParameter>?>(Parameters(deviceId).ProjectToType<Lib.App.DeviceParameter>());
    }

    public Task<IEnumerable<Client>?> ClientsByOwner(string ownerId)
    {
        return Task.FromResult<IEnumerable<Client>?>(_database.Clients.Where(x => x.OwnerId == ownerId)
            .ProjectToType<Lib.App.Client>());
    }

    public Task<IEnumerable<Device>?> DevicesByClient(Guid clientId)
    {
        return Task.FromResult<IEnumerable<Device>?>(_database.Devices.Include(x => x.SupportedDevice)
            .Where(x => x.ClientDeviceId == clientId).ProjectToType<Lib.App.Device>(DeviceAdapter.DTOConfig));
    }
}