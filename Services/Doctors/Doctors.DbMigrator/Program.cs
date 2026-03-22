using Doctors.Infrastructure;
using Shared.Infrastructure;

await using var migrator = new DbMigrator<DoctorsDbContext>(Consts.Assembly);
await migrator.MigrateAsync();

public static class Consts
{
    public const string Assembly = "Doctors.DbMigrator";
}

public class DoctorsDbContextFactory() : DbContextFactory<DoctorsDbContext>(Consts.Assembly);

