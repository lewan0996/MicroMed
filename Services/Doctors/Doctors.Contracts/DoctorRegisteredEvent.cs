namespace Doctors.Contracts;

public record DoctorRegisteredEvent(int Id, string FirstName, string LastName, int SpecialtyId);