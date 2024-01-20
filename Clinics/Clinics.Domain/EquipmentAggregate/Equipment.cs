using Shared.Domain;

namespace Clinics.Domain.EquipmentAggregate;

public class Equipment : Entity
{
    public EquipmentName Name { get; }

    private Equipment() { }
}

public record EquipmentName(string Value) : StringValueObject(Value, 20);