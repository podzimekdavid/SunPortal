namespace SunPortal.Cloud.Lib.App;

public class Client
{
    public Guid ClientId { get; set; }
    public string Name { get; set; }
    public int DevicesCount { get; set; }
    public string AllowedIpAddress { get; set; }
}