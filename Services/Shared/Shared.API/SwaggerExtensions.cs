using Microsoft.AspNetCore.Builder;

namespace Shared.API;

public static class SwaggerExtensions
{
    public static IApplicationBuilder UseSwaggerUIWithOpenApi(this WebApplication app)
    {
        app.MapOpenApi();
        return app.UseSwaggerUI(c => { c.SwaggerEndpoint("/openapi/v1.json", "v1"); });
    }
}