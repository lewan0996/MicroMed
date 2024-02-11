using Doctors.Contracts;
using Doctors.Domain.DoctorAggregate;
using MediatR;
using NServiceBus;
using Shared.Domain;
using Shared.Services;

namespace Doctors.Services;

public record RegisterDoctorCommand(Name Name, Specialty Specialty) : IRequest
{
    public RegisterDoctorCommand(string firstName, string lastName, int specialtyId) : this(new Name(firstName, lastName), Specialty.Get(specialtyId)) { }
}

public class RegisterDoctorCommandHandler : IRequestHandler<RegisterDoctorCommand>
{
    private readonly IDoctorsRepository _doctorsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageSession _messageSession;

    public RegisterDoctorCommandHandler(IDoctorsRepository doctorsRepository, IUnitOfWork unitOfWork, IMessageSession messageSession)
    {
        _doctorsRepository = doctorsRepository;
        _unitOfWork = unitOfWork;
        _messageSession = messageSession;
    }

    public async Task Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = new Doctor(request.Name, request.Specialty);

        await _doctorsRepository.AddDoctorAsync(doctor, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        //todo outbox
        await _messageSession.Publish(new DoctorRegisteredEvent(doctor.Id, doctor.Name.FirstName, doctor.Name.LastName), cancellationToken: cancellationToken);
    }
}