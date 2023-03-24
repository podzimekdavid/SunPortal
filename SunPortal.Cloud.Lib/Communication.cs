#define DEBUG
namespace SunPortal.Cloud.Lib;

public class Communication
{
    public const string RMQ_REQUEST_CHANNEL = "ValueRequests";
    public const string RMQ_RESPONSE_CHANNEL = "ValueRespons";
    public const string RMQ_CHANGE_PARAMETER_REQUEST_CHANNEL = "ChangeParameterReguests";
    public const string RMQ_CONFIG_PREFIX = "RabbitMQ:";

    public class Endpoints
    {
#if DEBUG
        public const string BASE = "http://10.30.1.20:7799";
#else
        public const string BASE = "http://database";
#endif

        public const string CLIENTS = "/clients";
        public const string PARAMETERS = "/device/parameters/";
        public const string DEVICES = "/devices/";
    }
}