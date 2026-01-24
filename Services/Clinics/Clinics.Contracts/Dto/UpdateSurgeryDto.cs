namespace Clinics.Contracts.Dto;

public record UpdateSurgeryDto(string Number, string Floor, IReadOnlyList<int> EquipmentIds);
