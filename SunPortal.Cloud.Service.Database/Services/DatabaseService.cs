using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Service.Database.Data;
using SunPortal.Communication.Parameters;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Service.Database.Adapters;
using Client = SunPortal.Cloud.Lib.App.Client;

namespace SunPortal.Cloud.Service.Database.Services;

public class DatabaseService
{
    private readonly DatabaseContext _database;

    public DatabaseService(DatabaseContext database)
    {
        _database = database;
    }

    // public IEnumerable<Lib.App.DeviceParameter> ParametersByDevice(Guid deviceId, ParameterPriority priority)
    // {
    //    // return Parameters(deviceId).Where(x=>x.)
    // }
    //
    private IQueryable<Parameter> Parameters(Guid deviceId)
    {
        var deviceType =
            _database.SupportedDevices.Include(x=>x.ParameterGroup)
                .ThenInclude(x=>x.Parameters)
                .FirstOrDefault(x =>
                    x.Devices.Any(x =>
                        x.ClientDeviceId == deviceId));

       // return deviceType.ParameterGroup.Parameters;
    }

    // public IEnumerable<Lib.App.DeviceParameter> ParametersByDevice(Guid deviceId)
    // {
    //     return Parameters(deviceId).ProjectToType<Lib.App.DeviceParameter>();
    // }

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