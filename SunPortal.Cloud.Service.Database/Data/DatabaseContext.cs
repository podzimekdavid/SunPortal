using Microsoft.EntityFrameworkCore;

namespace SunPortal.Cloud.Service.Database.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientDevice> Devices { get; set; }
    public DbSet<DeviceLog> Logs { get; set; }
    public DbSet<Parameter> Parameters { get; set; }
    public DbSet<ParameterGroup> ParameterGroups { get; set; }
    public DbSet<SupportedDevice> SupportedDevices { get; set; }
    public DbSet<GroupChart> Charts { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
}