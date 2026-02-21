using System.Net;
using MicroMed.BFF.Admin;
using MicroMed.BFF.Admin.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Syncfusion.Blazor;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

AddAuth();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

AddSyncfusionBlazor();

var serviceUrls = builder.Configuration.GetSection("Services").Get<ServiceUrls>()!;

builder.Services.AddSingleton(services
        => new ClinicsClient(serviceUrls.Clinics, services.GetRequiredService<IHttpClientFactory>()))
    .AddHttpClient();

var app = builder.Build();

app
    .UseDefaultFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

MapEndpoints();

app.Run();

void AddAuth()
{
    builder.Services
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
            options.ClientId = "admin-app";
            options.ClientSecret = config.Secret;
            options.ResponseType = "code";
            options.ResponseMode = "query";
            options.UsePkce = true;
            options.Scope.Add("admin-app");
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
            options.TokenValidationParameters = new()
            {
                NameClaimType = "name",
                ValidateAudience = true,
                ValidAudience = config.Audience,
                ValidateIssuer = true,
                ValidIssuer = config.Authority,
                ValidateLifetime = true
            };

            if (builder.Environment.IsDevelopment())
            {
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    Proxy = new WebProxy(config.Authority.Replace("localhost", "host.docker.internal")),
                };
            }
        });
}

void AddSyncfusionBlazor()
{
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(builder.Configuration["Syncfusion:LicenseKey"]);

    builder.Services.AddSyncfusionBlazor();
}

void MapEndpoints()
{
    app.MapGet("/check_session", (ClaimsPrincipal user) =>
    {
        if (user.Identity?.IsAuthenticated != true)
            return Results.Unauthorized();

        return Results.Ok(user.Claims.ToDictionary(claim => claim.Type, claim => claim.Value));
    });

    app.MapGet("/login", () => Results.Challenge(new AuthenticationProperties { RedirectUri = "/" })); // ??

    app.MapGet("/logout", () => Results.SignOut());
}

internal record ServiceUrls(string Clinics, string Doctors);

internal record AuthConfig(string Authority, string Audience, string Secret);
