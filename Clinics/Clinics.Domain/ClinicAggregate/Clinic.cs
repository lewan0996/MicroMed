using Shared.Domain;

namespace Clinics.Domain.ClinicAggregate;

public class Clinic : Entity, IAggregateRoot
{
    public ClinicName Name { get; }
    public Address Address { get; }

    private List<Surgery> _surgeries;
    public IReadOnlyCollection<Surgery> Surgeries => _surgeries;

    private Clinic()
    {
        _surgeries = new List<Surgery>();
    }
}