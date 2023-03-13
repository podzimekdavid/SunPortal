namespace SunPortal.Cloud.Service.Database.Data;

public class SupportedDevice
{
    public int SupportedDeviceId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ManufactureWebsiteUrl { get; set; }
    
    public int ParameterGroupId { get; set; }
    public virtual ParameterGroup ParameterGroup { get; set; }
    
    public virtual IEnumerable<ClientDevice> Devices { get; set; }
}