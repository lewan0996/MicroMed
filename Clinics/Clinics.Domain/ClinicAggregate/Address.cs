using Shared.Domain;

namespace Clinics.Domain.ClinicAggregate;

public record Address(
    City City,
    Street Street,
    StreetNumber Number,
    AddressAdditionalInformation AdditionalInformation) : ValueObject
{
    protected override void Validate() { }
}

public record City(string Value) : StringValueObject(Value, 20);
public record Street(string Value) : StringValueObject(Value, 50);
public record StreetNumber(string Value) : StringValueObject(Value, 4);
public record AddressAdditionalInformation(string Value) : StringValueObject(Value, 100);