namespace Clinics.Contracts.Dto;

public record AddSurgeryRequest(string Number, string Floor, IReadOnlyList<int> EquipmentIds);
