using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransit<TOutboxDbContext>(this IServiceCollection services,
        ConfigurationManager configuration) where TOutboxDbContext: DbContext =>
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(Assembly.Load).ToArray());

            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitConfig = configuration.GetSection("rabbitMq");

                cfg.Host(rabbitConfig["host"], rabbitConfig["virtualHost"], h =>
                {
                    h.Username(rabbitConfig["userName"]);
                    h.Password(rabbitConfig["password"]);
                });

                cfg.ConfigureEndpoints(context);
            });

            x.AddEntityFrameworkOutbox<TOutboxDbContext>(cfg =>
            {
                cfg.UseSqlServer();
                cfg.UseBusOutbox();
            });
        });
}