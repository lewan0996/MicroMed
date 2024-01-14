using Shared.Domain.Exceptions;

namespace Shared.Domain;

public record Name(FirstName FirstName, LastName LastName) : ValueObject
{
    public Name(string firstName, string lastName) : this(new FirstName(firstName), new LastName(lastName)) { }
    protected override void Validate() { }
}

public record FirstName(string Value) : ValueObject
{
    private const int MaxLength = 15;

    protected override void Validate()
    {
        if (Value.IsNullOrEmptyOrWhitespace())
            throw new DomainException("First name is required.");

        if (Value.Length > MaxLength)
            throw new DomainException($"First name is too long. Max length is {MaxLength}");
    }
}

public record LastName(string Value) : ValueObject
{
    private const int MaxLength = 50;

    protected override void Validate()
    {
        if (Value.IsNullOrEmptyOrWhitespace())
            throw new DomainException("Last name is required.");

        if (Value.Length > MaxLength)
            throw new DomainException($"Last name is too long. Max length is {MaxLength}");
    }
}