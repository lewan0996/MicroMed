using Clinics.Domain.ClinicAggregate;
using Clinics.Domain.EquipmentAggregate;
using Clinics.Services.Queries;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.EntityFramework;
// ReSharper disable UnassignedGetOnlyAutoProperty
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Clinics.Infrastructure;

public class ClinicsDbContext(DbContextOptions<ClinicsDbContext> options) : DbContextBase(options)
{
    public DbSet<Clinic> Clinics { get; init; }
    public DbSet<Equipment> Equipment { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var clinicsBuilder = modelBuilder.Entity<Clinic>();

        clinicsBuilder.ToTable(Tables.Clinics);

        clinicsBuilder.HasKey(x => x.Id);

        clinicsBuilder.HasStringValueObject(x => x.Name);

        clinicsBuilder.ComplexProperty(x => x.Address, x =>
            {
                x.HasStringValueObject(y => y.City);
                x.HasStringValueObject(y => y.Street);
                x.HasStringValueObject(y => y.Number, "street_number");
                x.HasStringValueObject(y => y.AdditionalInfo);
            })
            .HasMany(x => x.Surgeries).WithOne();

        clinicsBuilder.Navigation(x => x.Surgeries).AutoInclude();

        var surgeryBuilder = modelBuilder.Entity<Surgery>();

        surgeryBuilder.ToTable("surgeries")
            .ComplexProperty(x => x.SurgeryInfo, x =>
            {
                x.HasStringValueObject(y => y.Floor);
                x.HasStringValueObject(y => y.Number);

            })
            .HasMany(x => x.AvailableEquipment).WithMany().UsingEntity(x => x.ToTable("surgery_equipment"));

        surgeryBuilder.HasKey(x => x.Id);

        var equipmentBuilder = modelBuilder.Entity<Equipment>();

        equipmentBuilder.ToTable("equipment");

        equipmentBuilder.HasKey(x => x.Id);

        equipmentBuilder.HasStringValueObject(x => x.Name);

        modelBuilder.AddMassTransitOutbox();
    }
}