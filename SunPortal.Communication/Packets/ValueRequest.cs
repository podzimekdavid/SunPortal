namespace SunPortal.Communication.Packets;

public class ValueRequest
{
    public Guid RequestId { get; set; }
    public int Address { get; set; }
    public int ParameterId { get; set; }
}