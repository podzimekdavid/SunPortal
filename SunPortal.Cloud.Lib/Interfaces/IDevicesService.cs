using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Lib.Interfaces;

public interface IDevicesService
{
    public Task<IEnumerable<Lib.App.DeviceParameter>?> ParametersByDevice(Guid deviceId, ParameterPriority priority);

    public Task<IEnumerable<Lib.App.DeviceParameter>?> ParametersByDevice(Guid deviceId);

    public Task<IEnumerable<Lib.App.Client>?> ClientsByOwner(string ownerId);

    public Task<IEnumerable<Lib.App.Device>?> DevicesByClient(Guid clientId);

    public Task<Lib.App.Client?> Client(Guid clientId);

    public Task<Lib.App.Device?> Device(Guid deviceId);
}