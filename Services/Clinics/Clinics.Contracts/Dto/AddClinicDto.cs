namespace Clinics.Contracts.Dto;

public record AddClinicDto(string Name, string City, string Street, string StreetNumber, string? AdditionalInfo);
