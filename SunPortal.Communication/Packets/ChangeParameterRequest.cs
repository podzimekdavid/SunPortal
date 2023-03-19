namespace SunPortal.Communication.Packets;

public class ChangeParameterRequest:ValueRequest
{
    public byte[] Value { get; set; }
}