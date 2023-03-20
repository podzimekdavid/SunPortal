using System.Text;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SunPortal.Cloud.Lib.Extensions;
using SunPortal.Cloud.Lib.Models;
using SunPortal.Cloud.Service.Communication.Hub;
using SunPortal.Communication;
using SunPortal.Communication.Packets;
using IConnection = RabbitMQ.Client.IConnection;

namespace SunPortal.Cloud.Service.Communication.Services;

public class CommunicationService : IDisposable
{
    private readonly Dictionary<Guid, string> _clients = new();

    private readonly IConnection _connection;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<CommunicationHub> _hub;
    private IModel? _valueRequestChannel;
    private IModel? _valueResponseChannel;
    private IModel? _changeParameterRequestChannel;

    private readonly ILogger<CommunicationService> _log;

   

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
            HostName = _configuration.GetValue<string>($"{Lib.Communication.RMQ_CONFIG_PREFIX}Address"),
            UserName = _configuration.GetValue<string>($"{Lib.Communication.RMQ_CONFIG_PREFIX}User"),
            Password = _configuration.GetValue<string>($"{Lib.Communication.RMQ_CONFIG_PREFIX}Password"),
        };
    }

    private void Init()
    {
        _valueRequestChannel = _connection.CreateModel();
        _valueResponseChannel = _connection.CreateModel();
        _changeParameterRequestChannel = _connection.CreateModel();

        _valueRequestChannel.QueueDeclare(queue: Lib.Communication.RMQ_REQUEST_CHANNEL,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        _valueResponseChannel.QueueDeclare(queue: Lib.Communication.RMQ_RESPONSE_CHANNEL,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
        
        _changeParameterRequestChannel.QueueDeclare(queue: Lib.Communication.RMQ_CHANGE_PARAMETER_REQUEST_CHANNEL,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumerValue = new EventingBasicConsumer(_valueRequestChannel);
        consumerValue.Received += RequestReceived;

        _valueRequestChannel.BasicConsume(queue: Lib.Communication.RMQ_REQUEST_CHANNEL,
            autoAck: true,
            consumer: consumerValue);

        var consumerChange = new EventingBasicConsumer(_changeParameterRequestChannel);
        consumerChange.Received += ChangeParameterRequestReceived;

       _changeParameterRequestChannel.BasicConsume(queue: Lib.Communication.RMQ_CHANGE_PARAMETER_REQUEST_CHANNEL,
            autoAck: true,
            consumer: consumerChange);
       
    }
    
    private void ChangeParameterRequestReceived(object? model, BasicDeliverEventArgs args)
    {
        var request = args.Body.ToArray().ToObject<CommunicationChangeParameterRequest>();

        if (request == null)
            return;
        //_log.LogInformation($"Change request received {request.RequestId}");
        
        SendChangeParameterRequest(new()
        {
            RequestId = request.RequestId,
            Address = request.Address,
            ParameterId = request.Parameter,
            Value = request.Value
        },request.ClientId);
    }

    private void RequestReceived(object? model, BasicDeliverEventArgs args)
    {
        var request = args.Body.ToArray().ToObject<CommunicationValueRequest>();

        if (request == null)
            return;
        //_log.LogInformation($"Request received {request.RequestId}");

        SendRequest(new()
        {
            RequestId = request.RequestId,
            Address = request.Address,
            ParameterId = request.Parameter
        }, request.ClientId);
    }

    public void ResponseReceived(ValueResponse response)
    {
        CommunicationValueResponse model = new()
        {
            RequestId = response.RequestId,
            Data = response.Data,
            ReceivedData = DateTime.Now
        };
        
        _valueRequestChannel.BasicPublish(exchange: "",
            routingKey: Lib.Communication.RMQ_RESPONSE_CHANNEL,
            basicProperties: null,
            body: model.ToMessage());
    }

    public void AddClient(string connectionId, Guid clientId)
    {
        if (_clients.ContainsKey(clientId))
            _clients[clientId] = connectionId;
        else
            _clients.Add(clientId, connectionId);
        
        _log.LogInformation($"Client registered {clientId}, clients count {_clients.Count}");
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    private void SendRequest(ValueRequest request, Guid clientId)
    {
        if (!_clients.ContainsKey(clientId))
        {
            _log.LogError($"Client not found {clientId}");
            return;
        }
        
        var id = _clients[clientId];

        _hub.Clients.Client(id).SendAsync(Connection.ClientMethods.VALUE_REQUEST, request);
    }
    
    private void SendChangeParameterRequest(ChangeParameterRequest request, Guid clientId)
    {
        if (!_clients.ContainsKey(clientId))
        {
            _log.LogError($"Client not found {clientId}");
            return;
        }
        
        var id = _clients[clientId];

        _hub.Clients.Client(id).SendAsync(Connection.ClientMethods.CHANGE_PARAMETER_REQUEST, request);
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