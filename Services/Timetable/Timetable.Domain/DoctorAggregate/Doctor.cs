using Shared.Domain;

namespace Timetable.Domain.DoctorAggregate;

public class Doctor : Entity, IAggregateRoot
{
    public Name Name { get; init; }
    public Specialty Specialty { get; init; }

    public Doctor(Name name, Specialty specialty) => (Name, Specialty) = (name, specialty);

    protected Doctor() { }
}