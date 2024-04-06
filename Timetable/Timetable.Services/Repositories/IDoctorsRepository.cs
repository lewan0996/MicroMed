using Timetable.Domain.DoctorAggregate;

namespace Timetable.Services.Repositories;

public interface IDoctorsRepository
{
    Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken);
}