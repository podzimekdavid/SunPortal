using SunPortal.Cloud.Lib.UI;

namespace SunPortal.Cloud.Service.Database.Data;

public class GroupChart
{
    public int GroupChartId { get; set; }
    
    public ChartTypeYValue YType { get; set; }
    
    public int YParameterId { get; set; }
    public Parameter YParameter { get; set; }

    public int ParameterGroupId { get; set; }
    public ParameterGroup ParameterGroup { get; set; }
}