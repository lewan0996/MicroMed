using Clinics.Domain.EquipmentAggregate;

namespace Clinics.Services.Repositories;

public interface IEquipmentRepository
{
    Task AddAsync(Equipment equipment, CancellationToken cancellationToken);
    Task<IReadOnlyList<Equipment>> GetEquipmentAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
}