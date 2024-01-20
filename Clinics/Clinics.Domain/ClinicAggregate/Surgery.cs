using Clinics.Domain.EquipmentAggregate;
using Shared.Domain;

namespace Clinics.Domain.ClinicAggregate;

public class Surgery : Entity
{
    public SurgeryNumber Number { get; }
    public SurgeryFloor Floor { get; }

    private List<Equipment> _availableEquipment { get; }
    public IReadOnlyCollection<Equipment> AvailableEquipment => _availableEquipment;

    private Surgery()
    {
        _availableEquipment = new List<Equipment>();
    }
}

public record SurgeryNumber(string Value): StringValueObject(Value, 4);
public record SurgeryFloor(string Value): StringValueObject(Value, 10);