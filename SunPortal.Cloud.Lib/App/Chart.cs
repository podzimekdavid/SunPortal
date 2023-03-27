using SunPortal.Cloud.Lib.UI;

namespace SunPortal.Cloud.Lib.App;

public class Chart
{
    public string Name { get; set; }
    public ChartType ChartType { get; set; }
    public DeviceParameter PrimaryYParameter { get; set; }
    public DeviceParameter? SecondaryYParameter { get; set; }
}