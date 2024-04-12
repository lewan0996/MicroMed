using Clinics.Domain.EquipmentAggregate;
using Shared.Domain;

namespace Clinics.Domain.ClinicAggregate;

public class Surgery : Entity
{
    public SurgeryInfo SurgeryInfo { get; private set; }

    private List<Equipment> _availableEquipment { get; set; }
    public IReadOnlyCollection<Equipment> AvailableEquipment => _availableEquipment;

    public Surgery(SurgeryInfo surgeryInfo, IEnumerable<Equipment> equipment)
    {
        SurgeryInfo = surgeryInfo;
        _availableEquipment = equipment.ToList();
    }

    public void Update(SurgeryInfo surgeryInfo, IEnumerable<Equipment> equipment)
    {
        SurgeryInfo = surgeryInfo;
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