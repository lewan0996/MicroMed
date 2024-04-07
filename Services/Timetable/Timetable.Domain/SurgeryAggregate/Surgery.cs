using Shared.Domain;

namespace Timetable.Domain.SurgeryAggregate;

public class Surgery : Entity
{
    public string Floor { get; }
    public string Number { get; }

    public Surgery(string floor, string number)
    {
        Floor = floor;
        Number = number;
    }

    protected Surgery() { }
}