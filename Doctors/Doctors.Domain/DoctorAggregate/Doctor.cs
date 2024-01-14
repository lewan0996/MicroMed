using Shared.Domain;

namespace Doctors.Domain.DoctorAggregate;

public class Doctor : Entity<int>, IAggregateRoot
{
    public Name Name { get; init; }
    public Specialty Specialty { get; init; }

    public Doctor(Name name, Specialty specialty) => (Name, Specialty) = (name, specialty);

    protected Doctor() { }
}

public class Specialty : DictionaryType<Specialty>
{
    public static Specialty Internist = new(SpecialtyEnum.Internist, nameof(SpecialtyEnum.Internist));
    public static Specialty Pediatrician = new(SpecialtyEnum.Pediatrician, nameof(SpecialtyEnum.Pediatrician));
    public static Specialty Dermatologist = new(SpecialtyEnum.Dermatologist, nameof(SpecialtyEnum.Dermatologist));
    public static Specialty Surgeon = new(SpecialtyEnum.Surgeon, nameof(SpecialtyEnum.Surgeon));
    public static Specialty Nurse = new(SpecialtyEnum.Nurse, nameof(SpecialtyEnum.Nurse));
    public static Specialty Neurologist = new(SpecialtyEnum.Neurologist, nameof(SpecialtyEnum.Neurologist));
    public static Specialty Cardiologist = new(SpecialtyEnum.Cardiologist, nameof(SpecialtyEnum.Cardiologist));
    public static Specialty Orthopaedist = new(SpecialtyEnum.Orthopaedist, nameof(SpecialtyEnum.Orthopaedist));

    private Specialty(SpecialtyEnum id, string name) : base((int)id, name)
    {
    }

    public enum SpecialtyEnum
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
}

