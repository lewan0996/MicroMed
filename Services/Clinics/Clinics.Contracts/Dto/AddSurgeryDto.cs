namespace Clinics.Contracts.Dto;

public record AddSurgeryDto(string Number, string Floor, IReadOnlyList<int> EquipmentIds);
