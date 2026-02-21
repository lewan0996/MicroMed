using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Shared.API;

public static  class JwtExtensions
{
    public static AuthenticationBuilder AddAuth(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
        => services
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtOptions =>
            {
                var config = configuration.GetSection("Auth").Get<AuthConfig>()!;

                jwtOptions.Authority = config.Authority;
                jwtOptions.Audience = config.Audience;
                jwtOptions.MetadataAddress = config.MetadataUrl;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = config.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = config.Authority,
                    ValidateLifetime = true
                };
                jwtOptions.RequireHttpsMetadata = !environment.IsDevelopment();
            });
}