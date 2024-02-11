namespace Doctors.Contracts;

public record DoctorRegisteredEvent(int Id, string FirstName, string LastName); //todo move specialty to shared domain