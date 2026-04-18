using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.EntityFramework;

namespace Shared.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEfDbContextWithPostgres<TDbContext>(this IServiceCollection services,
        IConfiguration configuration) where TDbContext : DbContextBase =>
        services.AddDbContext<TDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Postgres"), npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure();
            })
            .UseSnakeCaseNamingConvention();
        });
    
    public static IServiceCollection AddMassTransit<TOutboxDbContext>(this IServiceCollection services,
        ConfigurationManager configuration) where TOutboxDbContext : DbContext =>
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(Assembly.Load).ToArray());

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitConfig = configuration.GetRequiredSection("rabbitMq");

                cfg.Host(rabbitConfig["host"], rabbitConfig["virtualHost"], h =>
                {
                    h.Username(rabbitConfig["userName"]!);
                    h.Password(rabbitConfig["password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
            
            x.AddEntityFrameworkOutbox<TOutboxDbContext>(cfg =>
            {
                cfg.UsePostgres();
                cfg.UseBusOutbox();
            });
        });
}