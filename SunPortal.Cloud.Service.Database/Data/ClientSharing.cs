namespace SunPortal.Cloud.Service.Database.Data;

public class ClientSharing
{
    public int ClientSharingId { get; set; }
    public Guid ClientId { get; set; }
    public string UserId { get; set; }

    public DateTimeOffset Created { get; set; }
}