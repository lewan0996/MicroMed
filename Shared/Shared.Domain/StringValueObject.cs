namespace Shared.Domain;

public abstract record StringValueObject(string Value, int MaxLength) : ValueObject
{
    protected override void Validate()
    {
        if (Value.Length > MaxLength)
            throw new ArgumentException($"{GetType().Name} cannot be longer than {MaxLength}.");
    }
}