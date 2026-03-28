using Timetable.Infrastructure;
using Shared.Infrastructure;

await using var migrator = new DbMigrator<TimetableDbContext>(Consts.Assembly);
await migrator.MigrateAsync();

public static class Consts
{
    public const string Assembly = "Timetable.DbMigrator";
}

public class TimetableDbContextFactory() : DbContextFactory<TimetableDbContext>(Consts.Assembly);

