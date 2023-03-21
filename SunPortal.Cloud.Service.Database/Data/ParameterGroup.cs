namespace SunPortal.Cloud.Service.Database.Data;

public class ParameterGroup
{
    public int ParameterGroupId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public virtual IEnumerable<SupportedDevice> SupportedDevices { get; set; }
    public virtual IEnumerable<Parameter> Parameters { get; set; }

}