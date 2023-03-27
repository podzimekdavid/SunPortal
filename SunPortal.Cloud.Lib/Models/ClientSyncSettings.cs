namespace SunPortal.Cloud.Lib.App;

public class ClientSyncSettings
{
    /// <summary>
    /// Sync interval - ms
    /// </summary>
    public int Interval { get; set; }

    /// <summary>
    ///  Device address, Parameters id collection
    /// </summary>
    public Dictionary<int, IEnumerable<int>> Parameters { get; set; } = new();
}