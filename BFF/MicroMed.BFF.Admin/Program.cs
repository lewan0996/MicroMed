using IdentityModel;
using MicroMed.BFF.Admin;
using MicroMed.BFF.Admin.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddGlobalExceptionHandler()
    .AddAuthorization()
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = "oidc";
        options.DefaultSignOutScheme = "oidc";
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("oidc", options =>
    {
        var config = builder.Configuration.GetSection("Auth").Get<AuthConfig>()!;

        options.Authority = config.Authority;
        options.ClientId = "AdminPortal";
        options.ClientSecret = config.Secret;
        options.ResponseType = "code";
        options.Scope.Add("admin");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;

        options.RequireHttpsMetadata = false;        
    });

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["Syncfusion:LicenseKey"]);

builder.Services.AddSyncfusionBlazor();
//builder.Services.AddServerSideBlazor();
//builder.Services.AddRazorPages();

builder.Services.AddBff();

var serviceUrls = builder.Configuration.GetSection("Services").Get<ServiceUrls>()!;

builder.Services.AddSingleton(services => new ClinicsClient(builder.Environment.IsDevelopment(), serviceUrls.Clinics));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app
    .UseDefaultFiles()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseBff()
    .UseAuthorization()
    .UseAntiforgery();

app.MapBffManagementEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseExceptionHandler();

app.Run();

return;

internal record ServiceUrls(string Clinics, string Doctors);

internal record AuthConfig(string Authority, string Secret);