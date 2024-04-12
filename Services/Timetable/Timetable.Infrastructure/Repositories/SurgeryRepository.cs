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
}