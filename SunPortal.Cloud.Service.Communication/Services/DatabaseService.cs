using Mapster;
using Microsoft.AspNetCore.SignalR;
using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Cloud.Service.Communication.Hub;
using SunPortal.Communication;
using SunPortal.Communication.Packets;

namespace SunPortal.Cloud.Service.Communication.Services;

public class DatabaseService
{
    private readonly HttpClient _client;
    private readonly IHubContext<CommunicationHub> _hub;

    public DatabaseService(IHubContext<CommunicationHub> hub)
    {
        _hub = hub;
        _client = new HttpClient();
        _client.BaseAddress = new Uri(Lib.Communication.Endpoints.BASE);
    }


    public async void ClientSettings(Guid clientId, string connectionId)
    {
        var result = await _client.GetFromJsonAsync<ClientSyncSettings>(
            $"{Lib.Communication.Endpoints.SYNC_SETTINGS}?clientId={clientId}");

        if (result == null)
            return;
        
        await _hub.Clients.Client(connectionId).SendAsync(Connection.ClientMethods.SET_SYNC_SETTINGS, result.Adapt<SyncSettings>());
    }

    public async void DeviceSync(SyncPackage package)
    {
        await _client.PostAsJsonAsync(Lib.Communication.Endpoints.DEVICE_SYNC,
            package.Adapt<DeviceSyncPackage>());
    }
}