using Shared.Domain;

namespace Clinics.Domain.ClinicAggregate;

public record ClinicName(string Value) : StringValueObject(Value, 50);