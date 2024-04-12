using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services,
        ConfigurationManager configuration) =>
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
        });
}