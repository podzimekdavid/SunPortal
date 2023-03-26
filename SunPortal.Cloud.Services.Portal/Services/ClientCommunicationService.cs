using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SunPortal.Cloud.Lib;
using SunPortal.Cloud.Lib.Extensions;
using SunPortal.Cloud.Lib.Models;

namespace SunPortal.Cloud.Services.Portal.Services;

public class ClientCommunicationService:IDisposable
{
    private readonly IConnection _connection;
    private readonly IConfiguration _configuration;
    
    /// <summary>
    ///  RequestId,DeviceId
    /// </summary>
   // private readonly Dictionary<Guid, Guid> _requests;
    private IModel? _valueRequestChannel;
    private IModel? _valueResponseChannel;
    private IModel? _changeParameterRequestChannel;

    private readonly ILogger<ClientCommunicationService> _log;

    public delegate void MQResponseReceived(CommunicationValueResponse response);

    public MQResponseReceived? OnResponseReceived;


    public ClientCommunicationService(IConfiguration configuration,
        ILogger<ClientCommunicationService> log)
    {
        _configuration = configuration;
        //_requests = new();
        _log = log;

        _connection = ConnectionFactory().CreateConnection();

        Init();
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
        
        var consumer = new EventingBasicConsumer(_valueResponseChannel);
        consumer.Received += ResponseReceived;

        _valueRequestChannel.BasicConsume(queue: Lib.Communication.RMQ_RESPONSE_CHANNEL,
            autoAck: true,
            consumer: consumer);
    }

    private void ResponseReceived(object? model, BasicDeliverEventArgs args)
    {
        var response = args.Body.ToArray().ToObject<CommunicationValueResponse>();

        if (response == null)
            return;

        // if (_requests.ContainsKey(response.RequestId))
        // {
            OnResponseReceived?.Invoke(response);

        //     _requests.Remove(response.RequestId);
        // }
        // else
        // {
        //     _log.LogWarning($"Lost response found - {response.RequestId}");
        // }
        
    }

    public void SendRequest(CommunicationValueRequest request)
    {
        //_requests.Add(request.RequestId, deviceId);

        _valueRequestChannel.BasicPublish(exchange: "",
            routingKey: Lib.Communication.RMQ_REQUEST_CHANNEL,
            basicProperties: null,
            body: request.ToMessage());
    }
    
    public void SendChangeParameterRequest(CommunicationChangeParameterRequest request, Guid deviceId)
    {
        //_log.LogError(request.RequestId.ToString());
        _changeParameterRequestChannel.BasicPublish(exchange: "",
            routingKey: Lib.Communication.RMQ_CHANGE_PARAMETER_REQUEST_CHANNEL,
            basicProperties: null,
            body: request.ToMessage());
    }

    public void Dispose()
    {
        _connection.Dispose();
        _valueRequestChannel?.Dispose();
        _valueResponseChannel?.Dispose();
        _changeParameterRequestChannel?.Dispose();
    }
}