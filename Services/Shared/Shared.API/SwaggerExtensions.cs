using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Shared.API;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
        => services.AddSwaggerGen(o =>
        {
            var config = configuration.GetAuthConfig();

            o.AddSecurityDefinition("OAuth2",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(config.AuthorizationUrl),
                            TokenUrl = new Uri(config.TokenUrl),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "profile", "profile" },
                                { config.Audience, "API access" }
                            }
                        },
                    }
                });

            o.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("OAuth2", doc),
                    ["openid", "profile", config.Audience]
                }
            });
        });

    public static IApplicationBuilder UseSwaggerUI(this WebApplication app)
    {
        var config = app.Configuration.GetAuthConfig();
        
        return app
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger");
                c.OAuthUsePkce();
                c.OAuthScopes("openid", "profile", config.Audience);
            });
    }
    
    private static AuthConfig GetAuthConfig(this IConfiguration configuration) 
        => configuration
               .GetSection("Auth")
               .Get<AuthConfig>()
           ?? throw new InvalidOperationException("Auth configuration section is missing.");
}