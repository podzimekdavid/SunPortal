using SunPortal.Cloud.Lib.UI;

namespace SunPortal.Cloud.Service.Database.Data;

public class GroupChart
{
    public int GroupChartId { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    
    public ChartTypeYValue YType { get; set; }
    public ChartType ChartType { get; set; }
    
    public int PrimaryYParameterId { get; set; }
    public Parameter PrimaryYParameter { get; set; }
    
    public int? SecondaryYParameterId { get; set; }
    public Parameter? SecondaryYParameter { get; set; }

    public int ParameterGroupId { get; set; }
    public ParameterGroup ParameterGroup { get; set; }
}