using SunPortal.Cloud.Service.Database.Data;
using SunPortal.Communication.Parameters;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Service.Database.Adapters;

namespace SunPortal.Cloud.Service.Database.Services;

public class DevicesService
{
    private readonly DatabaseContext _database;

    public DevicesService(DatabaseContext database)
    {
        _database = database;
    }

    public IEnumerable<Lib.App.DeviceParameter> ParametersByDevice(Guid deviceId, ParameterPriority priority)
    {
        return Parameters(deviceId).Where(x => x.Priority == priority).ProjectToType<Lib.App.DeviceParameter>();
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


    public IEnumerable<Lib.App.DeviceParameter> ParametersByDevice(Guid deviceId)
    {
        return Parameters(deviceId).ProjectToType<Lib.App.DeviceParameter>();
    }

    public IEnumerable<Lib.App.Client> ClientsByOwner(string ownerId)
    {
        return _database.Clients.Where(x => x.OwnerId == ownerId).ProjectToType<Lib.App.Client>();
    }

    public IEnumerable<Lib.App.Device> DevicesByClient(Guid clientId)
    {
        return _database.Devices.Include(x => x.SupportedDevice)
            .Where(x => x.ClientDeviceId == clientId).ProjectToType<Lib.App.Device>(DeviceAdapter.DTOConfig);
    }
}