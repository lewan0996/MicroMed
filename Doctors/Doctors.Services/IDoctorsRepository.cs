using Doctors.Domain.DoctorAggregate;

namespace Doctors.Services;

public interface IDoctorsRepository
{
    Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken);
}