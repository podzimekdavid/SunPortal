using SunPortal.Cloud.Service.Database.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Interfaces;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Cloud.Lib.Parameters;
using SunPortal.Cloud.Service.Database.Adapters;
using Client = SunPortal.Cloud.Lib.App.Client;

namespace SunPortal.Cloud.Service.Database.Services;

public class DevicesService : IDevicesService
{
    private readonly DatabaseContext _database;

    public DevicesService(DatabaseContext database)
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
        return Task.FromResult<IEnumerable<DeviceParameter>?>(Parameters(deviceId)
            .ProjectToType<Lib.App.DeviceParameter>());
    }

    public Task<IEnumerable<Client>?> ClientsByOwner(string ownerId)
    {
        return Task.FromResult<IEnumerable<Client>?>(_database.Clients.Include(x => x.Devices)
            .Where(x => x.OwnerId == ownerId)
            .ProjectToType<Lib.App.Client>(ClientAdapter.DTOConfig));
    }

    public Task<IEnumerable<Device>?> DevicesByClient(Guid clientId)
    {
        return Task.FromResult<IEnumerable<Device>?>(_database.Devices.Include(x => x.SupportedDevice)
            .Where(x => x.ClientId == clientId).ProjectToType<Lib.App.Device>(DeviceAdapter.DTOConfig));
    }

    public Task<Client?> Client(Guid clientId)
    {
        return Task.FromResult(_database.Clients.Include(x => x.Devices)
            .First(x => x.ClientId == clientId).Adapt<Lib.App.Client?>(ClientAdapter.DTOConfig));
    }

    public Task<Device?> Device(Guid deviceId)
    {
        return Task.FromResult<Device?>(_database.Devices.Include(x => x.SupportedDevice)
            .First(x => x.ClientDeviceId == deviceId).Adapt<Lib.App.Device>(DeviceAdapter.DTOConfig));
    }

    public Task<ClientSyncSettings> SyncSettings(Guid clientId)
    {
        ClientSyncSettings settings = new()
        {
            Interval = 75000 // TODO: add to database
        };

        foreach (var device in _database.Devices
                     .Include(x => x.SupportedDevice)
                     .ThenInclude(x => x.ParameterGroup)
                     .ThenInclude(x => x.Parameters)
                     .Where(x => x.ClientId == clientId))
        {
            List<int> parameters = new();

            foreach (Parameter parameter in device.SupportedDevice.ParameterGroup.Parameters.Where(x => x.LogParameter))
            {
                parameters.Add(parameter.ParameterId);
            }

            settings.Parameters.Add(device.Address, parameters);
        }

        return Task.FromResult(settings);
    }

    public Task DeviceSync(DeviceSyncPackage package)
    {
        var device =
            _database.Devices.FirstOrDefault(x => x.ClientId == package.ClientId && x.Address == package.Address);

        if (device == null)
            return Task.CompletedTask;

        foreach (KeyValuePair<int, byte[]> packageValue in package.Values)
        {
            _database.Logs.Add(new()
            {
                ClientDeviceId = device.ClientDeviceId,
                DateTime = DateTimeOffset.Now,
                ParameterId = packageValue.Key,
                Value = packageValue.Value
            });
        }


        _database.SaveChanges();

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Chart>> DeviceCharts(Guid deviceId)
    {
        return Task.FromResult<IEnumerable<Chart>>(
            _database.Charts.Include(x => x.ParameterGroup)
                .ThenInclude(x => x.SupportedDevices).ThenInclude(x => x.Devices)
                .Where(chart => chart.ParameterGroup
                    .SupportedDevices
                    .Any(x => x.Devices.Any(y => y.ClientDeviceId == deviceId)))
                .ProjectToType<Chart>(ChartAdapter.DTOConfig));
    }
}