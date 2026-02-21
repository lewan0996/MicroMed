using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared.API;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRWithTransactionBehavior(this IServiceCollection services)
        => services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblies(Assembly.GetEntryAssembly()!.GetReferencedAssemblies().Select(Assembly.Load).ToArray());
            x.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });
}

