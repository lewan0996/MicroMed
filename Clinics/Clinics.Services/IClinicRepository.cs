using Clinics.Domain.ClinicAggregate;

namespace Clinics.Services;

public interface IClinicRepository
{
    Task<Clinic?> GetOptionalAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Clinic clinic, CancellationToken cancellationToken);
}