using MatBlazor;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using SunPortal.Cloud.Lib.Interfaces;
using SunPortal.Cloud.Services.Portal.Areas.Identity;
using SunPortal.Cloud.Services.Portal.Data;
using SunPortal.Cloud.Services.Portal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration.GetValue<string>("Authentication:Google:ClientId");
        googleOptions.ClientSecret = builder.Configuration.GetValue<string>("Authentication:Google:ClientSecret");
        googleOptions.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(
                context.RedirectUri.Replace("http://", "https://", StringComparison.OrdinalIgnoreCase));
            return Task.CompletedTask;
        };
    })
    .AddCookie(options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<ClientCommunicationService>();
builder.Services.AddScoped<IDevicesService, DevicesCommunicationService>();

builder.Services.AddHttpClient();
builder.Services.AddMatBlazor();
//builder.Services.AddHttpContextAccessor();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
    options.RequireHeaderSymmetry = false;
    options.KnownProxies.Clear();
    options.KnownNetworks.Clear();
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseForwardedHeaders();

    // app.Use((context, next) =>
    // {
    //     context.Request.Host = new HostString(builder.Configuration.GetValue<string>("Domain:Default"));
    //     context.Request.Scheme = builder.Configuration.GetValue<string>("Domain:Scheme");
    //     return next();
    // });
}

app.Use((context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var protoHeaderValue) &&
        protoHeaderValue == "https")
    {
        context.Request.Scheme = "https";
    }

    return next();
});

app.UseHsts();


//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();