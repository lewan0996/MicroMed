using Shared.Domain;

namespace Doctors.Domain.DoctorAggregate;

public class Doctor : Entity<int>, IAggregateRoot
{
    public required Name Name { get; init; }
    public required Specialty Specialty { get; init; }

    public Doctor(Name name, Specialty specialty)
    {
        Name = name;
        Specialty = specialty;
    }

    protected Doctor() { }
}

public enum Specialty
{
    Internist = 1,
    Pediatrician = 2,
    Dermatologist = 3,
    Surgeon = 4,
    Nurse = 5,
    Neurologist = 6,
    Cardiologist = 7,
    Orthopaedist = 8
}