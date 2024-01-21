namespace Shared.Domain;

public record Name(FirstName FirstName, LastName LastName) : ValueObject
{
    public Name(string firstName, string lastName) : this(new FirstName(firstName), new LastName(lastName)) { }
    
    protected override void Validate() { }
}

public record FirstName(string Value) : StringValueObject(Value, 15);
public record LastName(string Value) : StringValueObject(Value, 50);