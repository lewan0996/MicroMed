using Shared.Domain;
using Shared.Domain.Exceptions;
using Timetable.Domain.DoctorAggregate;
using Timetable.Domain.SurgeryAggregate;

namespace Timetable.Domain.AppointmentAggregate;

public class Appointment : Entity
{
    public Doctor Doctor { get; }
    public Surgery Surgery { get; }
    public AppointmentDate Date { get; set; }

    public Appointment(Doctor doctor, Surgery surgery, AppointmentDate date)
    {
        Doctor = doctor;
        Surgery = surgery;
        Date = date;
    }

    protected Appointment() { }
}

public record AppointmentDate(DateTime DateTime, int DurationMinutes) : ValueObject
{
    protected override void Validate()
    {
        if (DurationMinutes <= 0)
            throw new DomainException("Appointment duration has to be greater than 0");
    }
}