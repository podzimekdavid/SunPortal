namespace SunPortal.Cloud.Lib.Models;

public class CommunicationValueResponse
{
    public DateTime ReceivedData { get; set; }
    public Guid RequestId { get; set; }
    public byte[]? Data { get; set; }
}