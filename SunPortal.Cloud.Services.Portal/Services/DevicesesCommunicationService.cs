using System.Collections;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Interfaces;
using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Services.Portal.Services;

public class DevicesesCommunicationService : IDevicesService
{
    private readonly HttpClient _client;

    public DevicesesCommunicationService()
    {
        _client = new();
        _client.BaseAddress = new(Lib.Communication.Endpoints.BASE);
    }

    private const string DEVICE_ID_ATRIBUTE = "deviceId=";

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
                ($"{Lib.Communication.Endpoints.DEVICES}?clientId={clientId}");
    }
}