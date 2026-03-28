using Shared.Domain;
#pragma warning disable CS8618

namespace Timetable.Domain.SurgeryAggregate;

public class Surgery : Entity
{
    public string Floor { get; private set; }
    public string Number { get; private set; }

    public Surgery(Guid id, string floor, string number) : base(id) => (Floor, Number) = (floor, number);
    

    protected Surgery() { }

    public void Update(string floor, string number)
    {
        Floor = floor;
        Number = number;
    }
}