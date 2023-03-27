using System.IO.Ports;
using System.Timers;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using StuderReader;
using SunPortal.Communication;
using SunPortal.Communication.Packets;
using Timer = System.Timers.Timer;

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
        _hub = new HubConnectionBuilder().WithUrl($"{Url}{Connection.HUB_PATH}", options =>
            {
                options.Transports =
                    HttpTransportType.WebSockets |
                    HttpTransportType.LongPolling;
            })
            .WithAutomaticReconnect(new[] {TimeSpan.Zero, TimeSpan.Zero, TimeSpan.FromSeconds(10)})
            .Build();

        _hub.On<ValueRequest>(Connection.ClientMethods.VALUE_REQUEST, ValueRequested);
        _hub.On<ChangeParameterRequest>(Connection.ClientMethods.CHANGE_PARAMETER_REQUEST, ChangeParameterRequested);
        _hub.On<SyncSettings>(Connection.ClientMethods.SET_SYNC_SETTINGS, SetSyncSettings);

        await _hub.StartAsync();

        Console.WriteLine(_hub.State);
        Register();
    }

    private Timer? _timer;
    private SyncSettings? _syncSettings;

    private void SetSyncSettings(SyncSettings settings)
    {
        if (_timer != null)
        {
            _timer.Stop();
        }

        _timer = new();
        _timer.Interval = settings.Interval;

        _timer.Elapsed += SyncTimer;
        _syncSettings = settings;
        
        if (_debug)
            Console.WriteLine($"Downloaded settings for {settings.Parameters.Count} devices.");

        _timer.Start();

        if (_debug)
            Console.WriteLine("Timer started");
    }

    private void SyncTimer(object? sender, ElapsedEventArgs e)
    {
        if (_syncSettings == null)
            return;

        if (_debug)
            Console.WriteLine($"{DateTime.Now} sync");

        foreach (KeyValuePair<int, IEnumerable<int>> device in _syncSettings.Parameters)
        {
            SyncPackage package = new()
            {
                Address = device.Key,
                ClientId = ClientId
            };

            foreach (var parameter in device.Value)
            {
                var data = ReadValue((ushort) parameter, device.Key);

                if (data == null)
                    return;
                package.Values.Add(parameter, data);
            }
            if (_debug)
                Console.WriteLine($"{DateTime.Now} package ready.");
            
            _hub.InvokeAsync(Connection.ServerMethods.SYNC, package).GetAwaiter().GetResult();
            
            if (_debug)
                Console.WriteLine($"{DateTime.Now} package send.");
        }
    }

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