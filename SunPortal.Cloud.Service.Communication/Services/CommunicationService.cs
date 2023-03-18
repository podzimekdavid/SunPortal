using System.Text;
using Microsoft.AspNet.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SunPortal.Cloud.Lib.Extensions;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Cloud.Service.Communication.Hub;
using SunPortal.Communication.Packets;
using IConnection = RabbitMQ.Client.IConnection;

namespace SunPortal.Cloud.Service.Communication.Services;

public class CommunicationService : IHostedService, IDisposable
{
    private readonly Dictionary<Guid, string> _clients = new();

    private readonly IConnection _connection;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<CommunicationHub> _hub;
    private IModel? _valueRequestChannel;

    private readonly ILogger<CommunicationService> _log;

    private const string RMQ_CONFIG_PREFIX = "RabbitMQ:";
    private const string RMQ_REQUEST_CHANNEL = "ValueRequests";

    public CommunicationService(IConfiguration configuration, ILogger<CommunicationService> log,
        IHubContext<CommunicationHub> hub)
    {
        _configuration = configuration;
        _log = log;
        _hub = hub;

        _connection = ConnectionFactory().CreateConnection();
    }

    private ConnectionFactory ConnectionFactory()
    {
        return new ConnectionFactory()
        {
            HostName = _configuration.GetValue<string>($"{RMQ_CONFIG_PREFIX}Address"),
            UserName = _configuration.GetValue<string>($"{RMQ_CONFIG_PREFIX}User"),
            Password = _configuration.GetValue<string>($"{RMQ_CONFIG_PREFIX}Password"),
        };
    }

    private void Init()
    {
        _valueRequestChannel = _connection.CreateModel();

        _valueRequestChannel.QueueDeclare(queue: RMQ_REQUEST_CHANNEL,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(_valueRequestChannel);
        consumer.Received += RequestReceived;
        
        _valueRequestChannel.BasicConsume(queue: RMQ_REQUEST_CHANNEL,
            autoAck: true,
            consumer: consumer);
    }

    private void RequestReceived(object? model, BasicDeliverEventArgs args)
    {
        var request = args.Body.ToArray().ToObject<CommunicationValueRequest>();

        if (request == null)
            return;

        SendRequest(new()
        {
            RequestId = request.RequestId,
            Address = request.Address,
            ParameterId = request.Parameter
        }, request.ClientId);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private void SendRequest(ValueRequest request, Guid clientId)
    {
        var id = _clients[clientId];

        _hub.Clients.All.Request(id, request);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Init();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        try
        {
            _connection.Close();
        }
        catch (Exception e)
        {
            _log.LogError("StopAsync method", e);
        }

        return Task.CompletedTask;
    }
}