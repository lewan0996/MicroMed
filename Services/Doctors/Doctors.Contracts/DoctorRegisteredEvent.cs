namespace Doctors.Contracts;

public record DoctorRegisteredEvent(Guid Id, string FirstName, string LastName, int SpecialtyId);