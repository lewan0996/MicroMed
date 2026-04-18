using Microsoft.EntityFrameworkCore;
using Shared.Services;

namespace Shared.Infrastructure.EntityFramework;

public class DbContextBase(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public Task<TResult> ExecuteInTransactionAsync<TResult>(Func<Task<TResult>> action,
        CancellationToken cancellationToken)
        => Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            
            try
            {
                var result = await action();

                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
}