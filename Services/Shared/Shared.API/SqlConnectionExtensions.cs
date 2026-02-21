using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared.API;

public static class SqlConnectionExtensions
{
    public static IServiceCollection AddSqlConnectionProvider(this IServiceCollection services,
        ConfigurationManager configuration)
        => services.AddScoped(_ => new SqlConnectionProvider(configuration.GetConnectionString("SqlServer")
                                                             ?? throw new InvalidOperationException(
                                                                 "Connection string 'SqlServer' not found.")));
}

