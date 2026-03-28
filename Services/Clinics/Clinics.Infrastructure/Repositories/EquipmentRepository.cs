using Clinics.Domain.EquipmentAggregate;
using Clinics.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;

namespace Clinics.Infrastructure.Repositories;

public class EquipmentRepository(ClinicsDbContext dbContext) : IEquipmentRepository
{
    public async Task AddAsync(Equipment equipment, CancellationToken cancellationToken) => await dbContext.AddAsync(equipment, cancellationToken);

    public async Task<IReadOnlyList<Equipment>> GetEquipmentAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        var result = await dbContext.Equipment.Where(x => ids.Contains(x.Id)).ToListAsync(cancellationToken);

        var notFoundEquipmentIds = ids.Where(id => result.All(y => y.Id != id)).ToList();

        return notFoundEquipmentIds.Count > 0 
            ? throw new ObjectNotFoundException($"Equipment of ids {string.Join(", ", notFoundEquipmentIds)} not found") 
            : result.ToList();
    }
}