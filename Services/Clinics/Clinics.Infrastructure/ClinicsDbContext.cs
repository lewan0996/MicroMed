using Clinics.Domain.ClinicAggregate;
using Clinics.Domain.EquipmentAggregate;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;
using Shared.Services;

namespace Clinics.Infrastructure;

public class ClinicsDbContext : DbContext, IUnitOfWork
{
    public DbSet<Clinic> Clinics { get; set; }
    public DbSet<Equipment> Equipment { get; set; }

    public ClinicsDbContext(DbContextOptions<ClinicsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var clinicsBuilder = modelBuilder.Entity<Clinic>();

        clinicsBuilder.ToTable("Clinics");

        clinicsBuilder.Property(x=>x.Name).HasConversion(x=>x.Value, value => new ClinicName(value)).HasColumnName("Name");
        clinicsBuilder.ComplexProperty(x => x.Address, x =>
            {
                x.HasStringValueObject(y => y.City);
                x.HasStringValueObject(y => y.Street, "StreetNumber");
                x.HasStringValueObject(y => y.Number);
                x.HasStringValueObject(y => y.AdditionalInformation);
            })
            .HasMany(x => x.Surgeries).WithOne();

        var surgeryBuilder = modelBuilder.Entity<Surgery>();

        surgeryBuilder.ToTable("Surgeries")
            .ComplexProperty(x => x.SurgeryInfo, x =>
            {
                x.HasStringValueObject(y => y.Floor);
                x.HasStringValueObject(y => y.Number);

            })
            .HasMany(x => x.AvailableEquipment).WithMany().UsingEntity(x => x.ToTable("SurgeryEquipment"));

        var equipmentBuilder = modelBuilder.Entity<Equipment>();

        equipmentBuilder.ToTable("Equipment").HasStringValueObject(x => x.Name);

        modelBuilder.AddMassTransitOutbox();
    }
}