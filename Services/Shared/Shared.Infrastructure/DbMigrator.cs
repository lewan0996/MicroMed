using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Shared.Infrastructure;

public class DbMigrator<TDbContext> : IAsyncDisposable where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;
    public DbMigrator(string migrationsAssembly)
    {
        var factory = new DbContextFactory<TDbContext>(migrationsAssembly);

        _dbContext = factory.CreateDbContext([]);
    }
    
    public async Task MigrateAsync() => await _dbContext.Database.MigrateAsync();

    public ValueTask DisposeAsync() => _dbContext.DisposeAsync();
}

public class DbContextFactory<TDbContext>(string migrationsAssembly)
    : IDesignTimeDbContextFactory<TDbContext> where TDbContext : DbContext
{
    public TDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json")
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseSqlServer(configuration.GetConnectionString("SqlServer"),
                b => b.MigrationsAssembly(migrationsAssembly))
            .Options;

        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), options)!;
    }
}