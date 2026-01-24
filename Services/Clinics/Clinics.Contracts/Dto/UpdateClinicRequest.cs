namespace Clinics.Contracts.Dto;

public record UpdateClinicRequest(string Name, string City, string Street, string StreetNumber, string? AdditionalInfo);
