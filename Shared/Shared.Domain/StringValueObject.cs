using Shared.Domain.Exceptions;

namespace Shared.Domain;

public abstract record StringValueObject(string Value, int MaxLength, bool IsRequired = true) : ValueObject
{
    protected override void Validate()
    {
        if (Value.Length > MaxLength)
            throw new DomainException($"{GetType().Name} cannot be longer than {MaxLength}.");

        if (IsRequired && Value.IsNullOrEmptyOrWhitespace())
            throw new DomainException($"{GetType().Name} is required");
    }
}