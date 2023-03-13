namespace SunPortal.Cloud.Service.Database.Data;

public class ClientDevice
{
    public Guid ClientDeviceId { get; set; }
    
    /// <summary>
    /// Studer communication address
    /// </summary>
    public int Address { get; set; }
    
    public int SupportedDeviceId { get; set; }
    public virtual SupportedDevice SupportedDevice { get; set; }
    
    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; }
    
    public virtual IEnumerable<DeviceLog> Logs { get; set; } 
}