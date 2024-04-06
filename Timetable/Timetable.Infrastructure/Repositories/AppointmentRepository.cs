using Timetable.Domain.AppointmentAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Infrastructure.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly TimetableDbContext _dbContext;

    public AppointmentRepository(TimetableDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        => await _dbContext.AddAsync(appointment, cancellationToken);
}