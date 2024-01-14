using Doctors.Domain.DoctorAggregate;
using MediatR;
using Shared.Domain;
using Shared.Services;

namespace Doctors.Services;

public class RegisterDoctorCommand : IRequest
{
    public Name Name { get; }
    public Specialty Specialty { get; }

    public RegisterDoctorCommand(string firstName, string lastName, int specialtyId)
    {
        Name = new Name(firstName, lastName);

        Specialty = Specialty.Get(specialtyId);
    }
}

public class RegisterDoctorCommandHandler : IRequestHandler<RegisterDoctorCommand>
{
    private readonly IDoctorsRepository _doctorsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterDoctorCommandHandler(IDoctorsRepository doctorsRepository, IUnitOfWork unitOfWork)
    {
        _doctorsRepository = doctorsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = new Doctor(request.Name, request.Specialty);

        await _doctorsRepository.AddDoctorAsync(doctor, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}