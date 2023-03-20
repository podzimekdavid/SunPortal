namespace SunPortal.Communication.Packets;

public class ValueResponse
{
    public Guid RequestId { get; set; }
    public Guid ClientId { get; set; }
    public byte[]? Data { get; set; }
}