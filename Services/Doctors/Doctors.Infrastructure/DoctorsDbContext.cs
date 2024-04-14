using Doctors.Domain.DoctorAggregate;
using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Infrastructure;

namespace Doctors.Infrastructure;

public class DoctorsDbContext(DbContextOptions<DoctorsDbContext> options) : DbContextBase(options)
{
    public DbSet<Doctor> Doctors { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var doctorsBuilder = modelBuilder.Entity<Doctor>();

        doctorsBuilder.ToTable("Doctors");

        doctorsBuilder.Property(x => x.Id).UseHiLo("doctoridseq");

        doctorsBuilder.ComplexProperty(x => x.Name, x =>
        {
            x.HasStringValueObject(y => y.FirstName);
            x.HasStringValueObject(y => y.LastName);
        });
        
        doctorsBuilder.Property(x => x.Specialty).HasConversion(x => x.Name, x => Specialty.Get(x));

        modelBuilder.AddMassTransitOutbox();
    }
}