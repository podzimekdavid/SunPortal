namespace SunPortal.Communication;

public class Connection
{
    public const string HUB_PATH = "/gateway";
    
    public class ClientMethods
    {
        public const string VALUE_REQUEST = "ValueRequested";
        public const string CHANGE_PARAMETER_REQUEST = "ChangeParameterRequested";
    }
    

    public class ServerMethods
    {
        public const string VALUE_RESPONSE = "ValueResponseReceived";
        public const string REGISTER = "Register";
    }
}