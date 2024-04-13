using Timetable.Domain.SurgeryAggregate;

namespace Timetable.Services.Repositories;

public interface ISurgeryRepository
{
    Task AddAsync(Surgery surgery, CancellationToken cancellationToken);
    Task<Surgery> GetAsync(int id, CancellationToken cancellationToken);
    Task RemoveAsync(int id, CancellationToken cancellationToken);
}