namespace SunPortal.Cloud.Service.Database.Data;

public class Client
{
    public Guid ClientId { get; set; }
    public string? AllowedIpAddress { get; set; }

    public string? Name { get; set; }
    
    public string OwnerId { get; set; }
    
    public virtual IEnumerable<ClientDevice> Devices { get; set; }
}