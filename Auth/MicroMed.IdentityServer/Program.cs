using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddAuthorization()
    .AddIdentityServer()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.BuildClients(builder.Configuration.GetSection("Clients").Get<ClientConfiguration[]>()!))
            .AddTestUsers(TestUsers.Users);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app
        .UseSwagger()
        .UseSwaggerUI();
}

app
    .UseStaticFiles()
    .UseRouting()
    .UseIdentityServer()
    .UseAuthorization();

app
    .MapRazorPages()
    .RequireAuthorization();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict }); // to work with http

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
            new ApiScope("patient")
        ];

    public static IEnumerable<Client> BuildClients(ClientConfiguration[] clientsConfiguration) =>
        clientsConfiguration.Select(clientConfig => new Client
        {
            ClientId = clientConfig.Name,
            ClientSecrets = { new Secret(clientConfig.Secret.Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,
            RedirectUris = { $"{clientConfig.BaseUrl}/signin-oidc" },
            PostLogoutRedirectUris = { $"{clientConfig.BaseUrl}signout-callback-oidc" },
            AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    clientConfig.Scope
                ]
        });        
}

internal record ClientConfiguration(string Name, string Scope, string BaseUrl, string Secret);