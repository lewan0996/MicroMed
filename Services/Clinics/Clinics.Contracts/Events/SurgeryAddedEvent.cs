namespace Clinics.Contracts.Events;

public record SurgeryAddedEvent(Guid Id, string Floor, string Number);