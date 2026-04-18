using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresConnectionProvider(this IServiceCollection services, IConfiguration configuration)
        => services.AddScoped(_ => new PostgresConnectionProvider(configuration.GetConnectionString("Postgres")
                                                             ?? throw new InvalidOperationException(
                                                                 "Connection string 'Postgres' not found.")));
    
    public static IServiceCollection AddMediatRWithTransactionBehavior(this IServiceCollection services)
        => services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblies(Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(Assembly.Load).ToArray());
            x.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
}