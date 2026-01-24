using Grpc.AspNetCore.Server;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Shared.Domain.Exceptions;

namespace Shared.API;

[Obsolete]
public class GlobalGrpcExceptionHandlerInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            if (exception is not DomainException and not ObjectNotFoundException)
                throw;

            var status = exception switch
            {
                DomainException ex => new Status(StatusCode.InvalidArgument, ex.Message),
                ObjectNotFoundException ex => new Status(StatusCode.NotFound, ex.Message),
                _ => throw new ArgumentOutOfRangeException(nameof(exception))
            };

            throw new RpcException(status, exception.Message);
        }
    }
}