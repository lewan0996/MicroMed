using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Clinics.Domain.ClinicAggregate;

public class Clinic : Entity, IAggregateRoot
{
    public ClinicName Name { get; }
    public Address Address { get; }

    private List<Surgery> _surgeries;
    public IReadOnlyCollection<Surgery> Surgeries => _surgeries;

    public Clinic(ClinicName name, Address address) : this()
    {
        Name = name;
        Address = address;
    }

    private Clinic()
    {
        _surgeries = new List<Surgery>();
    }

    public void AddSurgery(Surgery surgery)
    {
        if (Surgeries.Any(x => x.SurgeryInfo == surgery.SurgeryInfo))
        {
            throw new DomainException(
                $"Surgery with number {surgery.SurgeryInfo.Number.Value} already exists in clinic {Name.Value}, on floor {surgery.SurgeryInfo.Floor.Value}");
        }

        _surgeries.Add(surgery);
    }
}