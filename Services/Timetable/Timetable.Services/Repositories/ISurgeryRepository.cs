using Timetable.Domain.SurgeryAggregate;

namespace Timetable.Services.Repositories;

public interface ISurgeryRepository
{
    Task AddSurgeryAsync(Surgery surgery, CancellationToken cancellationToken);
}