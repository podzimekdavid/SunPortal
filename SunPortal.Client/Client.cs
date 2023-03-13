using System.IO.Ports;
using Microsoft.AspNetCore.SignalR.Client;
using StuderReader;
using SunPortal.Communication;
using SunPortal.Communication.Packets;

namespace SunPortal.Client;

public class Client
{
    private HubConnection _hub;

    private Studer _studer;

    public Client(string comPort)
    {
        _studer = new Studer(comPort);
    }

    public string Url { get; set; }
    public Guid ClientId { get; set; }

    public async void Start()
    {
        _hub = new HubConnectionBuilder().WithUrl($"{Url}{Connection.HUB_PATH}")
            .WithAutomaticReconnect(new[] {TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10)})
            .Build();

        _hub.On<ValueRequest>(Connection.ClientMethods.VALUE_REQUEST, ValueRequested);

        await _hub.StartAsync();
    }

    private async void ValueRequested(ValueRequest request)
    {
        await _hub.InvokeAsync(Connection.ServerMethods.VALUE_RESPONSE, new ValueResponse()
        {
            RequestId = request.RequestId,
            ClientId = ClientId,
            Data = ReadValue((ushort) request.ParameterId, request.Address)
        });
    }

    private object? ReadValue(ushort parameterId, int address)
    {
        lock (_studer)
        {
            Frame frame = new(Address.Me,
                Address.Inverter(address),
                OperationType.Read,
                new UserInfo(parameterId, UserInfo.Property.Value));

            var data = _studer.SendAndReceiveFrame(frame);

            return data?.Object;
        }
    }
}