using Clinics.Domain.EquipmentAggregate;
using Shared.Domain;
using Shared.Domain.Exceptions;

namespace Clinics.Domain.ClinicAggregate;

public class Clinic : Entity, IAggregateRoot
{
    public ClinicName Name { get; private set; }
    public Address Address { get; private set; }

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

    public void Update(ClinicName name, Address address)
    {
        Name = name;
        Address = address;
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

    public void UpdateSurgery(int surgeryId, SurgeryInfo surgeryInfo, IEnumerable<Equipment> equipment)
    {
        var surgery = GetSurgery(surgeryId);

        surgery.Update(surgeryInfo, equipment);
    }

    public void RemoveSurgery(int surgeryId)
    {
        var surgery = GetSurgery(surgeryId);

        _surgeries.Remove(surgery);
    }

    private Surgery GetSurgery(int surgeryId)
    {
        var surgery = Surgeries.SingleOrDefault(x => x.Id == surgeryId);

        if (surgery == null)
        {
            throw new ObjectNotFoundException("Surgery of given id does not exist in the clinic");
        }

        return surgery;
    }
}