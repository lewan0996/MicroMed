namespace Clinics.Contracts.Dto;

public record AddClinicRequest(string Name, string City, string Street, string StreetNumber, string? AdditionalInfo);
