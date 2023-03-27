namespace SunPortal.Cloud.Service.Database.Data;

public class DeviceLog
{
    public int DeviceLogId { get; set; }
    public byte[] Value { get; set; }
    public DateTimeOffset DateTime { get; set; }
    
    public int ParameterId { get; set; }
    public virtual Parameter Parameter { get; set; }
    
    public Guid ClientDeviceId { get; set; }
    public virtual ClientDevice ClientDevice { get; set; }
    
}