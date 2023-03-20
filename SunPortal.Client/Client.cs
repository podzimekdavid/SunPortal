using System.IO.Ports;
using Microsoft.AspNetCore.SignalR.Client;
using StuderReader;
using SunPortal.Communication;
using SunPortal.Communication.Packets;

namespace SunPortal.Client;

public class Client : IDisposable
{
    private HubConnection _hub;

    private readonly Studer _studer;
    private bool _debug;

    public Client(string comPort,bool debug = false )
    {
        _debug = debug;
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
        _hub.On<ChangeParameterRequest>(Connection.ClientMethods.CHANGE_PARAMETER_REQUEST, ChangeParameterRequested);

        await _hub.StartAsync();

        Console.WriteLine(_hub.State);
        Register();
        
    }

    private void ChangeParameterRequested(ChangeParameterRequest request)
    {
        if (_debug)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(request.ParameterId);
            Console.WriteLine(request.Address);
            //Console.WriteLine(BitConverter.ToBoolean(request.Value, 0));
            Console.ResetColor();

        }

        SetParameter((ushort) request.ParameterId, request.Address, request.Value);
    }

    private async void Register()
    {
        await _hub.InvokeAsync(Connection.ServerMethods.REGISTER, ClientId);
    }

    private async void ValueRequested(ValueRequest request)
    {
        if (_debug)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(request.ParameterId);
            Console.ResetColor();
        }

        await _hub.InvokeAsync(Connection.ServerMethods.VALUE_RESPONSE, new ValueResponse()
        {
            RequestId = request.RequestId,
            ClientId = ClientId,
            Data = ReadValue((ushort) request.ParameterId, request.Address)
        });
    }

    private byte[]? ReadValue(ushort parameterId, int address)
    {
        lock (_studer)
        {
            Frame frame = new(Address.Me,
                Address.Inverter(address),
                OperationType.Read,
                new UserInfo(parameterId, UserInfo.Property.Value));

            //Console.WriteLine(address);
            var data = _studer.SendAndReceiveFrame(frame);


            //Console.WriteLine(BitConverter.ToSingle(data.Object.Data, 0));
            return data.Object.Data;
        }
    }

    private void SetParameter(ushort parameterId, int address, byte[] value)
    {
        lock (_studer)
        {
            _studer.SendAndReceiveFrame(new Frame(Address.Me, Address.Inverter(address), OperationType.Write,
                new Parameter(parameterId, Parameter.Property.UnsavedValueQsp, value)));
        }
    }

    public void Dispose()
    {
        //TODO: logout
    }
}