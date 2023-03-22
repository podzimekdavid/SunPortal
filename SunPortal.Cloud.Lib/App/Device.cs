namespace SunPortal.Cloud.Lib.App;

public class Device
{
    public Guid ClientDeviceId { get; set; }
    public int Address { get; set; }
    public int SupportedDeviceId { get; set; }
    public Guid ClientId { get; set; }
    
    public DeviceType DeviceType { get; set; }
}