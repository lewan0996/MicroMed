using Clinics.Infrastructure;
using Shared.Infrastructure;

await using var migrator = new DbMigrator<ClinicsDbContext>(Consts.Assembly);
await migrator.MigrateAsync();

public static class Consts
{
    public const string Assembly = "Clinics.DbMigrator";
}

public class ClinicsDbContextFactory() : DbContextFactory<ClinicsDbContext>(Consts.Assembly);