using Timetable.Domain.SurgeryAggregate;

namespace Timetable.Services.Repositories;

public interface ISurgeryRepository
{
    Task AddAsync(Surgery surgery, CancellationToken cancellationToken);
    Task<Surgery> GetAsync(Guid id, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}