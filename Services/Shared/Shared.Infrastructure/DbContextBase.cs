using Microsoft.EntityFrameworkCore;
using Shared.Services;

namespace Shared.Infrastructure;

public class DbContextBase(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public Task BeginTransactionAsync(CancellationToken cancellationToken) => Database.BeginTransactionAsync(cancellationToken);

    public Task CommitTransactionAsync(CancellationToken cancellationToken) => Database.CommitTransactionAsync(cancellationToken);

    public Task RollbackTransactionAsync(CancellationToken cancellationToken) => Database.RollbackTransactionAsync(cancellationToken);
}