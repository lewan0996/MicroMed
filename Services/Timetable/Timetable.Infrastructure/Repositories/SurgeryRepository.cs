using Microsoft.EntityFrameworkCore;
using Shared.Domain.Exceptions;
using Timetable.Domain.SurgeryAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Infrastructure.Repositories;

public class SurgeryRepository : ISurgeryRepository
{
    private readonly TimetableDbContext _dbContext;

    public SurgeryRepository(TimetableDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Surgery surgery, CancellationToken cancellationToken)
        => await _dbContext.Surgeries.AddAsync(surgery, cancellationToken);

    public async Task<Surgery> GetAsync(int id, CancellationToken cancellationToken)
        => await _dbContext.Surgeries.SingleOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new ObjectNotFoundException("Surgery of given id does not exist");

    public async Task RemoveAsync(int id, CancellationToken cancellationToken)
    {
        var surgeryToRemove = await GetAsync(id, cancellationToken);

        _dbContext.Surgeries.Remove(surgeryToRemove);
    }
}