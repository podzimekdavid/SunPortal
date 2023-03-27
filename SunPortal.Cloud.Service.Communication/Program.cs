using Microsoft.AspNetCore.Http.Connections;
using SunPortal.Cloud.Service.Communication.Hub;
using SunPortal.Cloud.Service.Communication.Services;
using SunPortal.Communication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(hubOptions => hubOptions.EnableDetailedErrors = true);
builder.Services.AddSingleton<CommunicationService>();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddHostedService<SyncService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<CommunicationHub>(Connection.HUB_PATH, options =>
{
    options.Transports =
        HttpTransportType.WebSockets |
        HttpTransportType.LongPolling;
});
app.MapControllers();

app.Run();