using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using SunPortal.Cloud.Service.Communication.Services;
using SunPortal.Communication;
using SunPortal.Communication.Packets;

namespace SunPortal.Cloud.Service.Communication.Hub;

public class CommunicationHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly CommunicationService _communicationService;
    private readonly DatabaseService _databaseService;
    private ILogger<CommunicationHub> _log;

    public CommunicationHub(ILogger<CommunicationHub> log, CommunicationService communicationService,
        DatabaseService databaseService)
    {
        _log = log;
        _communicationService = communicationService;
        _databaseService = databaseService;
    }


    public void ValueResponseReceived(ValueResponse response)
    {
        _communicationService.ResponseReceived(response);
    }

    public async void Register(Guid clientId)
    {
        _log.LogInformation($"Hub client register {clientId}");
        _communicationService.AddClient(Context.ConnectionId, clientId);

        var syncSettings = await _databaseService.ClientSettings(clientId);

        if (syncSettings != null)
        {
            await Clients.Caller.SendAsync(Connection.ClientMethods.SET_SYNC_SETTINGS, syncSettings);
        }
    }

    public void Sync(SyncPackage package)
    {
        _databaseService.DeviceSync(package);
    }

    public override Task OnConnectedAsync()
    {
        _log.LogInformation($"Hub client connected {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }
}