using Mapster;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Communication.Packets;

namespace SunPortal.Cloud.Service.Communication.Services;

public class DatabaseService
{
    private readonly HttpClient _client;

    public DatabaseService()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Lib.Communication.Endpoints.BASE);
    }


    public async Task<SyncSettings?> ClientSettings(Guid clientId)
    {
        var result = await _client.GetFromJsonAsync<ClientSyncSettings>(
            $"{Lib.Communication.Endpoints.SYNC_SETTINGS}?clientId={clientId}");

        if (result == null)
            return null;

        return result.Adapt<SyncSettings>();
    }

    public async void DeviceSync(SyncPackage package)
    {
        await _client.PostAsJsonAsync(Lib.Communication.Endpoints.DEVICE_SYNC,
            package.Adapt<DeviceSyncPackage>());
    }
}