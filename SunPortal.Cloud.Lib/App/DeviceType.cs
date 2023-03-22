namespace SunPortal.Cloud.Lib.App;

public class DeviceType
{
    public int SupportedDeviceId { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ManufactureWebsiteUrl { get; set; }
    public int ParameterGroupId { get; set; }
}