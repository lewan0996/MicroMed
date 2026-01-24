using Shared.Domain;
#pragma warning disable CS8618

namespace Doctors.Domain.DoctorAggregate;

public class Doctor : Entity, IAggregateRoot
{
    public Name Name { get; init; }
    public Specialty Specialty { get; init; }

    public Doctor(Name name, Specialty specialty) => (Name, Specialty) = (name, specialty);

    protected Doctor() { }
}