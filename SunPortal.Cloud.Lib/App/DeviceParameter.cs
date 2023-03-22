using SunPortal.Communication.Parameters;

namespace SunPortal.Cloud.Lib.App;

public class DeviceParameter
{
    public int ParameterId { get; set; }

    public string? Description { get; set; }

    public ParameterType Type { get; set; }
    public ParameterMode Mode { get; set; }
    public ParameterLevel Level { get; set; }
    public ParameterPriority Priority { get; set; }

    public int ParameterGroupId { get; set; }

    public string? Unit { get; set; }
    public bool LogParameter { get; set; } = false;
}