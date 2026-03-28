using Clinics.Domain.ClinicAggregate;
using Clinics.Domain.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;

namespace Clinics.Infrastructure;

public class ClinicsDbContext(DbContextOptions<ClinicsDbContext> options) : DbContextBase(options)
{
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Equipment> Equipment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var clinicsBuilder = modelBuilder.Entity<Clinic>();

        clinicsBuilder.ToTable("Clinics");

        clinicsBuilder.HasKey(x => x.Id);

        clinicsBuilder.HasStringValueObject(x => x.Name);

        clinicsBuilder.ComplexProperty(x => x.Address, x =>
            {
                x.HasStringValueObject(y => y.City);
                x.HasStringValueObject(y => y.Street);
                x.HasStringValueObject(y => y.Number, "StreetNumber");
                x.HasStringValueObject(y => y.AdditionalInfo);
            })
            .HasMany(x => x.Surgeries).WithOne();

        clinicsBuilder.Navigation(x => x.Surgeries).AutoInclude();

        var surgeryBuilder = modelBuilder.Entity<Surgery>();

        surgeryBuilder.ToTable("Surgeries")
            .ComplexProperty(x => x.SurgeryInfo, x =>
            {
                x.HasStringValueObject(y => y.Floor);
                x.HasStringValueObject(y => y.Number);

            })
            .HasMany(x => x.AvailableEquipment).WithMany().UsingEntity(x => x.ToTable("SurgeryEquipment"));

        surgeryBuilder.HasKey(x => x.Id);

        var equipmentBuilder = modelBuilder.Entity<Equipment>();

        equipmentBuilder.ToTable("Equipment");

        equipmentBuilder.HasKey(x => x.Id);

        equipmentBuilder.HasStringValueObject(x => x.Name);

        modelBuilder.AddMassTransitOutbox();
    }
}