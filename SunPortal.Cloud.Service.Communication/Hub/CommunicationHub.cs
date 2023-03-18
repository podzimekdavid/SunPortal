using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using SunPortal.Cloud.Service.Communication.Services;
using SunPortal.Communication;
using SunPortal.Communication.Packets;

namespace SunPortal.Cloud.Service.Communication.Hub;

public class CommunicationHub : Microsoft.AspNetCore.SignalR.Hub
{
    
    private readonly CommunicationService _service;

    public CommunicationHub(CommunicationService service)
    {
        _service = service;
    }

    public async void Request(string connectionString, ValueRequest request)
    {
        await Clients.Client(connectionString).SendAsync(Connection.ClientMethods.VALUE_REQUEST,request);
    }

    /*public async Task<ValueResponse?> Request(ValueRequest request, Guid clientId)
    {
        if (_clients.ContainsKey(clientId))
        {
            return await WaitForResponse();
        }

        return null;
    }

    private TaskCompletionSource<ValueResponse> _response;

    private Task<ValueResponse> WaitForResponse()
    {
        _response = new TaskCompletionSource<ValueResponse>();
        return _response.Task;
    }*/

    public void ValueResponseReceived(ValueResponse response)
    {
        //_response.SetResult(response);
    }

    public void Register(Guid clientId)
    {
        /*if (_clients.ContainsKey(clientId))
            _clients[clientId] = Context.ConnectionId;
        else
            _clients.Add(clientId, Context.ConnectionId);*/
    }
}