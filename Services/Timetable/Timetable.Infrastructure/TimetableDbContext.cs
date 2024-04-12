using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Infrastructure;
using Shared.Services;
using Timetable.Domain.AppointmentAggregate;
using Timetable.Domain.DoctorAggregate;
using Timetable.Domain.SurgeryAggregate;

namespace Timetable.Infrastructure;

public class TimetableDbContext : DbContext, IUnitOfWork
{
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Surgery> Surgeries { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;

    public TimetableDbContext(DbContextOptions<TimetableDbContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var doctorsBuilder = modelBuilder.Entity<Doctor>();

        doctorsBuilder.ToTable("Doctors");
        doctorsBuilder.ComplexProperty(x => x.Name, x =>
        {
            x.HasStringValueObject(y => y.FirstName);
            x.HasStringValueObject(y => y.LastName);
        });

        doctorsBuilder.Property(x => x.Specialty).HasConversion(x => x.Name, x => Specialty.Get(x));

        var surgeryBuilder = modelBuilder.Entity<Surgery>();

        surgeryBuilder.ToTable("Surgeries");
        surgeryBuilder.Property(x => x.Floor);
        surgeryBuilder.Property(x => x.Number);

        var appointmentBuilder = modelBuilder.Entity<Appointment>();
        appointmentBuilder.HasOne(x => x.Doctor).WithMany();
        appointmentBuilder.HasOne(x => x.Surgery).WithMany();
        appointmentBuilder.ComplexProperty(x => x.Date, x =>
        {
            x.Property(y => y.DurationMinutes);
            x.Property(y => y.DateTime);
        });

        modelBuilder.AddMassTransitOutbox();
    }
}