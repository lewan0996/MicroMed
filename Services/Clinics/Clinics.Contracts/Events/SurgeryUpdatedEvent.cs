namespace Clinics.Contracts.Events;

public record SurgeryUpdatedEvent(Guid Id, string Floor, string Number);