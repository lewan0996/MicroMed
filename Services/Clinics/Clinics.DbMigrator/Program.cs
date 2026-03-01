using Clinics.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Development.json")
    .AddEnvironmentVariables()
    .Build();

var options = new DbContextOptionsBuilder<ClinicsDbContext>()
    .UseSqlServer(configuration.GetConnectionString("SqlServer"))
    .Options;

await using var context = new ClinicsDbContext(options);
await context.Database.MigrateAsync();

public class ClinicsDbContextFactory : IDesignTimeDbContextFactory<ClinicsDbContext>
{
    public ClinicsDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ClinicsDbContext>()
            .UseSqlServer(b => b.MigrationsAssembly("Clinics.DbMigrator"))
            .Options;

        return new ClinicsDbContext(options);
    }
}