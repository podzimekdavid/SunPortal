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

    public Client(string comPort, bool debug = false)
    {
        _debug = debug;
        _studer = new Studer(comPort);
    }

    public string Url { get; set; }
    public Guid ClientId { get; set; }

    public async void Start()
    {
        _requests = new();
        _hub = new HubConnectionBuilder().WithUrl($"{Url}{Connection.HUB_PATH}")
            .WithAutomaticReconnect(new[] {TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10)})
            .Build();

        _hub.On<ValueRequest>(Connection.ClientMethods.VALUE_REQUEST, ValueRequested);
        _hub.On<ChangeParameterRequest>(Connection.ClientMethods.CHANGE_PARAMETER_REQUEST, ChangeParameterRequested);

        await _hub.StartAsync();

        Console.WriteLine(_hub.State);
        Register();
    }

    //public void CheckConnection()
    // {
    //     if (_hub.State != HubConnectionState.Connected)
    //     {
    //         Start();
    //
    //         Thread.Sleep(1000);
    //     }
    // }

    public void CheckQueue()
    {
        if (_requests.TryDequeue(out ValueRequest? request))
        {
            if (request == null)
                return;

            var data = ReadValue((ushort) request.ParameterId, request.Address);

            _hub.InvokeAsync(Connection.ServerMethods.VALUE_RESPONSE, new ValueResponse()
            {
                RequestId = request.RequestId,
                ClientId = ClientId,
                Data = data
            }).GetAwaiter().GetResult();

            if (_debug)
            {
                Console.WriteLine($"Send response {request.ParameterId}");
            }
        }
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

    private Queue<ValueRequest> _requests;

    private void ValueRequested(ValueRequest request)
    {
        if (_debug)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(request.ParameterId);
            Console.ResetColor();
        }

        _requests.Enqueue(request);
    }

    private byte[]? ReadValue(ushort parameterId, int address)
    {
        lock (_studer)
        {
            Frame frame = new(Address.Me,
                Address.Inverter(address),
                OperationType.Read,
                new UserInfo(parameterId, UserInfo.Property.Value));

            var data = _studer.SendAndReceiveFrame(frame);

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