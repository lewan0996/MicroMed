using Timetable.Domain.AppointmentAggregate;

namespace Timetable.Services.Repositories;

public interface IAppointmentRepository
{
    Task AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
}