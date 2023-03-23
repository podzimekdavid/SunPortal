using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Lib.Interfaces;
using SunPortal.Cloud.Service.Database.Data;
using SunPortal.Cloud.Service.Database.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options => { options.UseNpgsql(connectionString); });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DatabaseContext>();

builder.Services.AddScoped<IDevicesService, DevicesService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();