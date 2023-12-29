using Doctors.Domain.DoctorAggregate;

namespace Doctors.API.Contracts;

public record RegisterDoctorDto(string FirstName, string LastName, Specialty Specialty); //todo how to enforce valid enum values?