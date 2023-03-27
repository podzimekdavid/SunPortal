namespace SunPortal.Communication.Packets;

public class SyncPackage
{
    public Guid ClientId { get; set; }
    
    /// <summary>
    /// Device address
    /// </summary>
    public int Address { get; set; }

    /// <summary>
    /// Parameter id, value
    /// </summary>
    public Dictionary<int, byte[]> Values { get; set; } = new();
}