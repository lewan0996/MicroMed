using Shared.Domain.Exceptions;

namespace Shared.Domain;

public class Name : ValueObject
{
    public FirstName FirstName { get; init; }
    public LastName LastName { get; init; }

    public Name(string firstName, string lastName)
    {
        FirstName = new FirstName(firstName);
        LastName = new LastName(lastName);
    }

    protected override IEnumerable<IComparable?> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}

public class FirstName : ValueObject
{
    private const int MaxLength = 15;

    public string Value { get; init; }

    public FirstName(string value)
    {
        if (value.Length > MaxLength)
            throw new DomainException($"First name is too long. Max length is {MaxLength}");

        Value = value;
    }

    protected override IEnumerable<IComparable?> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class LastName : ValueObject
{
    private const int MaxLength = 50;

    public string Value { get; init; }

    public LastName(string value)
    {
        if (value.Length > MaxLength)
            throw new DomainException($"Last name is too long. Max length is {MaxLength}");

        Value = value;
    }

    protected override IEnumerable<IComparable?> GetEqualityComponents()
    {
        yield return Value;
    }
}