using MicroMed.BFF.Admin;
using MicroMed.BFF.Admin.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Syncfusion.Blazor;
using System.Security.Claims;

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
    .AddCookie()
    .AddOpenIdConnect("oidc", options =>
    {
        var config = builder.Configuration.GetSection("Auth").Get<AuthConfig>()!;

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = config.Authority;
        options.ClientId = "AdminPortal";
        options.ClientSecret = config.Secret;
        options.ResponseType = "code";
        options.ResponseMode = "query";
        options.UsePkce = true;
        options.Scope.Add("admin");
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new()
        {
            NameClaimType = "name"
        };
    });

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["Syncfusion:LicenseKey"]);

builder.Services.AddSyncfusionBlazor();

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
    .UseAuthorization()
    .UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseExceptionHandler();

app.MapGet("/check_session", (ClaimsPrincipal user) =>
{
    if (user.Identity?.IsAuthenticated != true)
        return Results.Unauthorized();

    return Results.Ok(user.Claims.ToDictionary(claim => claim.Type, claim => claim.Value));
});

app.MapGet("/login", () => Results.Challenge(new AuthenticationProperties { RedirectUri = "/" })); // ??

app.MapGet("/logout", () => Results.SignOut());

app.Run();

return;

internal record ServiceUrls(string Clinics, string Doctors);

internal record AuthConfig(string Authority, string Secret);