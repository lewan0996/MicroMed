using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Timetable.Domain.SurgeryAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Infrastructure.Repositories;

public class SurgeryRepository(TimetableDbContext dbContext) : ISurgeryRepository
{
    public async Task AddAsync(Surgery surgery, CancellationToken cancellationToken)
        => await dbContext.Surgeries.AddAsync(surgery, cancellationToken);

    public async Task<Surgery> GetAsync(Guid id, CancellationToken cancellationToken)
        => await dbContext.Surgeries.SingleOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new ObjectNotFoundException("Surgery of given id does not exist");

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        var surgeryToRemove = await GetAsync(id, cancellationToken);

        dbContext.Surgeries.Remove(surgeryToRemove);
    }
}