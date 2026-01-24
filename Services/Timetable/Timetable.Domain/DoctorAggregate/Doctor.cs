using Shared.Domain;
#pragma warning disable CS8618

namespace Timetable.Domain.DoctorAggregate;

public class Doctor : Entity, IAggregateRoot
{
    public Name Name { get; init; }
    public Specialty Specialty { get; init; }

    public Doctor(int id, Name name, Specialty specialty) : base(id) => (Name, Specialty) = (name, specialty);

    protected Doctor() { }
}