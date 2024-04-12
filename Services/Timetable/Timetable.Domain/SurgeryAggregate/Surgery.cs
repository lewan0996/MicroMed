using Shared.Domain;

namespace Timetable.Domain.SurgeryAggregate;

public class Surgery : Entity
{
    public string Floor { get; }
    public string Number { get; }

    public Surgery(Guid id, string floor, string number) : base(id)
    {
        Floor = floor;
        Number = number;
    }

    protected Surgery() { }
}