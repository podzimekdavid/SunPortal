namespace SunPortal.Cloud.Lib.Models;

public class CommunicationValueRequest
{
    public Guid RequestId { get; set; }
    public Guid ClientId { get; set; }
    
    public int Address { get; set; }
    public int Parameter { get; set; }
}