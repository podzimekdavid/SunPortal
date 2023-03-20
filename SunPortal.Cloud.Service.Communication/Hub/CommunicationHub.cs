using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using SunPortal.Cloud.Service.Communication.Services;
using SunPortal.Communication;
using SunPortal.Communication.Packets;

namespace SunPortal.Cloud.Service.Communication.Hub;

public class CommunicationHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly CommunicationService _communicationService;
    private ILogger<CommunicationHub> _log;

    public CommunicationHub(ILogger<CommunicationHub> log, CommunicationService communicationService)
    {
        _log = log;
        _communicationService = communicationService;
    }


    public void ValueResponseReceived(ValueResponse response)
    {
        _communicationService.ResponseReceived(response);
    }

    public void Register(Guid clientId)
    {
        _log.LogInformation($"Hub client register {clientId}");
        _communicationService.AddClient(Context.ConnectionId, clientId);
    }

    public override Task OnConnectedAsync()
    {
        _log.LogInformation($"Hub client connected {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }
}