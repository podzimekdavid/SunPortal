namespace SunPortal.Communication;

public class Connection
{
    public const string HUB_PATH = "/gateway";
    
    public class ClientMethods
    {
        public const string VALUE_REQUEST = "ValueRequested";
    }
    

    public class ServerMethods
    {
        public const string VALUE_RESPONSE = "ValueResponseReceived";
    }
}