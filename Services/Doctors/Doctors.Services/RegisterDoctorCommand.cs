using Doctors.Contracts;
using Doctors.Contracts.Dto;
using Doctors.Domain.DoctorAggregate;
using MassTransit;
using MediatR;
using Shared.Domain;
using Shared.Services;

namespace Doctors.Services;

public record RegisterDoctorCommand(Name Name, Specialty Specialty) : IRequest<Guid>
{
    public RegisterDoctorCommand(string firstName, string lastName, int specialtyId) 
        : this(new Name(firstName, lastName), Specialty.Get(specialtyId)) { }
    
    public RegisterDoctorCommand(RegisterDoctorDto dto) 
        : this(new Name(dto.FirstName, dto.LastName), Specialty.Get(dto.SpecialtyId)) { }
}

public class RegisterDoctorCommandHandler(
    IDoctorsRepository doctorsRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<RegisterDoctorCommand, Guid>
{
    public async Task<Guid> Handle(RegisterDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = new Doctor(request.Name, request.Specialty);

        await doctorsRepository.AddDoctorAsync(doctor, cancellationToken);

        await publishEndpoint.Publish(new DoctorRegisteredEvent(doctor.Id, doctor.Name.FirstName, doctor.Name.LastName, doctor.Specialty.Id), cancellationToken: cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return doctor.Id;
    }
}