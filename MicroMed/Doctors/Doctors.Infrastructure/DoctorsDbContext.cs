using Doctors.Domain.DoctorAggregate;
using Doctors.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Services;

namespace Doctors.Infrastructure;

public class DoctorsDbContext : DbContext, IUnitOfWork
{
    public DbSet<Doctor> Doctors { get; set; } = null!;

    public DoctorsDbContext(DbContextOptions<DoctorsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var doctorsBuilder = modelBuilder.Entity<Doctor>();

        doctorsBuilder.ToTable("Doctors");
        doctorsBuilder.OwnsOne(x => x.Name, x =>
        {
            x.Property(y => y.FirstName).HasConversion(y => y.Value, value => new FirstName(value)).HasColumnName("FirstName");
            x.Property(y => y.LastName).HasConversion(y => y.Value, value => new LastName(value)).HasColumnName("LastName");
        });
        doctorsBuilder.Property(x => x.Specialty).HasConversion<string>();
    }
}