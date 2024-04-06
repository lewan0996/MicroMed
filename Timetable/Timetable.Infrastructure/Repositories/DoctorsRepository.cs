using Timetable.Domain.DoctorAggregate;
using Timetable.Services.Repositories;

namespace Timetable.Infrastructure.Repositories;

public class DoctorsRepository : IDoctorsRepository
{
    private readonly TimetableDbContext _dbContext;

    public DoctorsRepository(TimetableDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken) 
        => await _dbContext.Doctors.AddAsync(doctor, cancellationToken);
}