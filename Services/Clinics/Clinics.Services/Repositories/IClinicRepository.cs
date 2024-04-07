using Clinics.Domain.ClinicAggregate;

namespace Clinics.Services.Repositories;

public interface IClinicRepository
{
    Task<Clinic> GetAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Clinic clinic, CancellationToken cancellationToken);
}