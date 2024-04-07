using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Shared.API;

public static class WebApplicationBuilderExtensions
{
    public static IHostBuilder ConfigureNServiceBusEndpoint(this WebApplicationBuilder builder, string endpointName) =>
        builder.Host.UseNServiceBus(_ =>
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);

            endpointConfiguration.EnableInstallers();

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningEventsAs(type => (type.Namespace?.EndsWith("Contracts") ?? false) && type.Name.EndsWith("Event"));

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology(QueueType.Quorum);
            transport.ConnectionString(builder.Configuration.GetConnectionString("RabbitMQ"));

            return endpointConfiguration;
        });
}