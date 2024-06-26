﻿using Microsoft.EntityFrameworkCore;
using Shared.Domain;
using Shared.Infrastructure;
using Timetable.Domain.AppointmentAggregate;
using Timetable.Domain.DoctorAggregate;
using Timetable.Domain.SurgeryAggregate;

namespace Timetable.Infrastructure;

public class TimetableDbContext(DbContextOptions<TimetableDbContext> options) : DbContextBase(options)
{
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<Surgery> Surgeries { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var doctorsBuilder = modelBuilder.Entity<Doctor>();

        doctorsBuilder.ToTable("Doctors");

        doctorsBuilder.Property(x => x.Id).ValueGeneratedNever();

        doctorsBuilder.ComplexProperty(x => x.Name, x =>
        {
            x.HasStringValueObject(y => y.FirstName);
            x.HasStringValueObject(y => y.LastName);
        });

        doctorsBuilder.Property(x => x.Specialty).HasConversion(x => x.Name, x => Specialty.Get(x));

        var surgeryBuilder = modelBuilder.Entity<Surgery>();

        surgeryBuilder.ToTable("Surgeries");

        surgeryBuilder.Property(x => x.Id).ValueGeneratedNever();

        surgeryBuilder.Property(x => x.Floor);
        surgeryBuilder.Property(x => x.Number);

        var appointmentBuilder = modelBuilder.Entity<Appointment>();

        appointmentBuilder.HasKey(x => x.Id);
        appointmentBuilder.HasOne(x => x.Doctor).WithMany();
        appointmentBuilder.HasOne(x => x.Surgery).WithMany();
        appointmentBuilder.ComplexProperty(x => x.Date, x =>
        {
            x.Property(y => y.DurationMinutes);
            x.Property(y => y.DateTime);
        });

        modelBuilder.AddMassTransitOutbox();
        //todo base entitybuilder
    }
}