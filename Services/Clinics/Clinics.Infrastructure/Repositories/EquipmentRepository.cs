using Clinics.Domain.EquipmentAggregate;
using Clinics.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;

namespace Clinics.Infrastructure.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly ClinicsDbContext _dbContext;

    public EquipmentRepository(ClinicsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Equipment equipment, CancellationToken cancellationToken) => await _dbContext.AddAsync(equipment, cancellationToken);

    public async Task<IReadOnlyList<Equipment>> GetEquipmentAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Equipment.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        var notFoundEquipmentIds = ids.Where(id => result.All(y => y.Id != id)).ToList();

        if (notFoundEquipmentIds.Any())
            throw new ObjectNotFoundException($"Equipment of ids {string.Join(", ", notFoundEquipmentIds)} not found");

        return result.ToList();
    }
}