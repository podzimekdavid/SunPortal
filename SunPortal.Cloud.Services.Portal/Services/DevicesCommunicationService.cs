using System.Collections;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Interfaces;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Services.Portal.Services;

public class DevicesCommunicationService : IDevicesService
{
    private readonly HttpClient _client;

    public DevicesCommunicationService()
    {
        _client = new();
        _client.BaseAddress = new(Lib.Communication.Endpoints.BASE);
    }

    private const string DEVICE_ID_ATRIBUTE = "deviceId=";
    private const string CLIENT_ID_ATRIBUTE = "clientId=";

    public async Task<IEnumerable<DeviceParameter>?> ParametersByDevice(Guid deviceId, ParameterPriority priority)
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<DeviceParameter>>
                ($"{Lib.Communication.Endpoints.PARAMETERS}?{DEVICE_ID_ATRIBUTE}{deviceId}&priority={priority}");
    }

    public async Task<IEnumerable<DeviceParameter>?> ParametersByDevice(Guid deviceId)
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<DeviceParameter>>
                ($"{Lib.Communication.Endpoints.PARAMETERS}?{DEVICE_ID_ATRIBUTE}{deviceId}");
    }

    public async Task<IEnumerable<Client>?> ClientsByOwner(string ownerId)
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<Client>>
                ($"{Lib.Communication.Endpoints.CLIENTS}?ownerId={ownerId}");
    }

    public async Task<IEnumerable<Device>?> DevicesByClient(Guid clientId)
    {
        return await _client
            .GetFromJsonAsync<IEnumerable<Device>?>
                ($"{Lib.Communication.Endpoints.DEVICES}?{CLIENT_ID_ATRIBUTE}{clientId}");
    }

    public async Task<Client?> Client(Guid clientId)
    {
        return await _client
            .GetFromJsonAsync<Client>
                ($"{Lib.Communication.Endpoints.CLIENT}?{CLIENT_ID_ATRIBUTE}{clientId}");
    }

    public async Task<Device?> Device(Guid deviceId)
    {
        return await _client
            .GetFromJsonAsync<Device>
                ($"{Lib.Communication.Endpoints.DEVICE}?{DEVICE_ID_ATRIBUTE}{deviceId}");
    }
}