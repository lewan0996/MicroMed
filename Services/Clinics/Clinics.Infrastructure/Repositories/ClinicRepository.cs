using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;

namespace Clinics.Infrastructure.Repositories;

public class ClinicRepository : IClinicRepository
{
    private readonly ClinicsDbContext _dbContext;

    public ClinicRepository(ClinicsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Clinic> GetAsync(int id, CancellationToken cancellationToken) 
        => (_dbContext.Clinics
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new ObjectNotFoundException($"Clinic of id {id} not found"))!;

    public async Task AddAsync(Clinic clinic, CancellationToken cancellationToken)
        => await _dbContext.Clinics.AddAsync(clinic, cancellationToken);
}