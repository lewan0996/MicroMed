using Clinics.Domain.EquipmentAggregate;
using Shared.Domain;

namespace Clinics.Domain.ClinicAggregate;

public class Surgery : Entity
{
    public SurgeryInfo SurgeryInfo { get; }

    private List<Equipment> _availableEquipment { get; }
    public IReadOnlyCollection<Equipment> AvailableEquipment => _availableEquipment;

    public Surgery(SurgeryNumber number, SurgeryFloor floor, IEnumerable<Equipment> equipment)
    {
        SurgeryInfo = new SurgeryInfo(number, floor);
        _availableEquipment = equipment.ToList();
    }

    private Surgery()
    {
        _availableEquipment = new List<Equipment>();
    }
}

public record SurgeryInfo(SurgeryNumber Number, SurgeryFloor Floor);
public record SurgeryNumber(string Value): StringValueObject(Value, 4);
public record SurgeryFloor(string Value): StringValueObject(Value, 10);