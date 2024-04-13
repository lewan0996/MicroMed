using Shared.Domain;

namespace Timetable.Domain.SurgeryAggregate;

public class Surgery : Entity
{
    public string Floor { get; private set; }
    public string Number { get; private set; }

    public Surgery(int id, string floor, string number) : base(id) => (Floor, Number) = (floor, number);
    

    protected Surgery() { }

    public void Update(string floor, string number)
    {
        Floor = floor;
        Number = number;
    }
}