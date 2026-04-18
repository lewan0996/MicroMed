using MediatR;

namespace Shared.Services;

public class TransactionBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
        => unitOfWork.ExecuteInTransactionAsync(() => next(cancellationToken), cancellationToken);
}