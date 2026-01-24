namespace Clinics.Contracts.Dto;

public record UpdateClinicDto(string Name, string City, string Street, string StreetNumber, string? AdditionalInfo);
