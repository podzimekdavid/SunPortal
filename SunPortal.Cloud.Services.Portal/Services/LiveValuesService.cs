using SunPortal.Cloud.Lib.App;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Cloud.Lib.Parameters;

namespace SunPortal.Cloud.Services.Portal.Services;

public class LiveValuesService : IDisposable
{
    private readonly ClientCommunicationService _communication;
    private readonly Dictionary<Guid, ParameterContext> _requests;

    public delegate void ResponseReceived(ParameterContext context, CommunicationValueResponse response);

    public event ResponseReceived? OnResponseReceived;

    public LiveValuesService(ClientCommunicationService communication)
    {
        _requests = new();

        _communication = communication;

        _communication.OnResponseReceived += OnClientResponseReceived;
    }

    public void SendRequest(CommunicationValueRequest request, ParameterContext context)
    {
        _requests.Add(request.RequestId, context);

        _communication.SendRequest(request);
    }

    private void OnClientResponseReceived(CommunicationValueResponse response)
    {
        if (_requests.ContainsKey(response.RequestId))
        {
            OnResponseReceived?.Invoke(_requests[response.RequestId], response);

            _requests.Remove(response.RequestId);
        }
    }


    public void Dispose()
    {

        _communication.OnResponseReceived -= OnClientResponseReceived;

    }
}