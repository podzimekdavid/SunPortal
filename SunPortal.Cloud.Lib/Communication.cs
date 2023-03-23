namespace SunPortal.Cloud.Lib;

public class Communication
{
    public const string RMQ_REQUEST_CHANNEL = "ValueRequests";
    public const string RMQ_RESPONSE_CHANNEL = "ValueRespons";
    public const string RMQ_CHANGE_PARAMETER_REQUEST_CHANNEL = "ChangeParameterReguests";
    public const string RMQ_CONFIG_PREFIX = "RabbitMQ:";
    
    public class Endpoints
    {
        public const string BASE = "http://database";
        public const string CLIENTS = "/clients";
        public const string PARAMETERS = "/device/parameters/";
        public const string DEVICES = "/devices/";
    }
}