namespace Clinics.Contracts.Dto;

public record UpdateSurgeryRequest(string Number, string Floor, IReadOnlyList<Guid> EquipmentIds);
