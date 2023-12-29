using Doctors.Domain.DoctorAggregate;
using Doctors.Services;

namespace Doctors.Infrastructure;

public class DoctorsRepository : IDoctorsRepository
{
    private readonly DoctorsDbContext _dbContext;

    public DoctorsRepository(DoctorsDbContext dbContext) => _dbContext = dbContext;

    public async Task AddDoctorAsync(Doctor doctor, CancellationToken cancellationToken) 
        => await _dbContext.Doctors.AddAsync(doctor, cancellationToken);
}