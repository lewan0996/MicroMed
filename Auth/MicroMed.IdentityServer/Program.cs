using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityModel;
using MicroMed.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Shared.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddAuthorization()
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services
    .AddIdentityServer()
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryClients(Config.BuildClients(builder.Configuration.GetSection("Clients").Get<ClientConfiguration[]>()!))
    .AddAspNetIdentity<IdentityUser>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetSection("SqlServer")["ConnectionString"];
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await EnsureDbCreated();

    app.UseSwaggerUIWithOpenApi();

    async Task EnsureDbCreated()
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        var adminConfig = builder.Configuration.GetSection("Admin").Get<AdminConfiguration>()!;

        await InsertRoleAsync(adminConfig.UserName);
        await InsertRoleAsync("doctor");
        await InsertRoleAsync("patient");

        await InsertAdminUserAsync();        

        async Task InsertRoleAsync(string roleName)
        {
            var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var role = await roleMgr.FindByNameAsync(roleName);

            if (role == null)
            {
                role = new IdentityRole
                {
                    Name = roleName
                };

                var result = await roleMgr.CreateAsync(role);

                ValidateResult(result);
            }
        }

        async Task InsertAdminUserAsync()
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var adminUser = await userMgr.FindByNameAsync(adminConfig.UserName);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminConfig.UserName,
                    Email = "admin@micromed.com",
                    EmailConfirmed = true
                };

                var result = await userMgr.CreateAsync(adminUser, adminConfig.Password);

                ValidateResult(result);

                result = await userMgr.AddClaimsAsync(adminUser, [
                            new Claim(JwtClaimTypes.Name, "admin"),
                        ]);

                ValidateResult(result);

                result = await userMgr.AddToRoleAsync(adminUser, "Admin");

                ValidateResult(result);
            }
        }

        void ValidateResult(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }    
}

app
    .MapStaticAssets();

app
    .UseRouting()
    .UseIdentityServer()
    .UseAuthorization();

app
    .MapRazorPages()
    .RequireAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict }); // to work with http

app.UseMiddleware<FixAntiForgeryIssueMiddleware>("/account/login");

app.Run();

internal static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("admin"),
            new ApiScope("doctor"),
            new ApiScope("patient"),
            new ApiScope("clinics.api", "Clinics API"),
            new ApiScope("doctors.api", "Doctors API"),
            new ApiScope("timetable.api", "Timetable API")
        ];

    public static IEnumerable<ApiResource> ApiResources =>
        [
            new ApiResource("clinics.api", "Clinics API")
            {
                Scopes = { "clinics.api" }
            },
            new ApiResource("doctors.api", "Doctors API")
            {
                Scopes = { "doctors.api" }
            },
            new ApiResource("timetable.api", "Timetable API")
            {
                Scopes = { "timetable.api" }
            }
        ];

    public static IEnumerable<Client> BuildClients(ClientConfiguration[] clientsConfiguration) =>
        clientsConfiguration.Select(clientConfig => new Client
        {
            ClientId = clientConfig.Name,
            ClientSecrets = { new Secret(clientConfig.Secret.Sha256()) },
            RequirePkce = true,
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { $"{clientConfig.BaseUrl}/signin-oidc" },
            PostLogoutRedirectUris = { $"{clientConfig.BaseUrl}signout-callback-oidc" },
            AllowedScopes = GetAllowedScopes(clientConfig)
        });

    private static List<string> GetAllowedScopes(ClientConfiguration clientConfig)
    {
        var scopes = new List<string>
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile,
            clientConfig.Scope
        };

        // Admin portal gets access to all APIs
        if (clientConfig.Name == "AdminPortal")
        {
            scopes.Add("clinics.api");
            scopes.Add("doctors.api");
            scopes.Add("timetable.api");
        }

        return scopes;
    }
}

internal record ClientConfiguration(string Name, string Scope, string BaseUrl, string Secret);

internal record AdminConfiguration(string UserName, string Password);