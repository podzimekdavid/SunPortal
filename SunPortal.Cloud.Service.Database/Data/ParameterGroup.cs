using SunPortal.Cloud.Lib.UI;

namespace SunPortal.Cloud.Service.Database.Data;

public class ParameterGroup
{
    public int GroupParameterId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public virtual IEnumerable<SupportedDevice> SupportedDevices { get; set; }
    public virtual IEnumerable<Parameter> Parameters { get; set; }
    public virtual IEnumerable<GroupChart> Charts { get; set; }

}