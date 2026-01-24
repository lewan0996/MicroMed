namespace Clinics.Contracts.Dto;

public record GetClinicsResponse(List<ClinicDto> Clinics);

public record ClinicDto(int Id, string Name, string City, string Street, string StreetNumber, string? AdditionalInfo);