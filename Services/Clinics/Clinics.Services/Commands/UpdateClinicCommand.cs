using Clinics.Contracts.Dto;
using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record UpdateClinicCommand(int ClinicId, ClinicName Name, Address Address) : IRequest
{
    public UpdateClinicCommand(int clinicId, string name, string city, string street, string streetNumber, string additionalInfo)
        : this(
            clinicId,
            new ClinicName(name),
            new Address(
                new City(city),
                new Street(street),
                new StreetNumber(streetNumber),
                new AddressAdditionalInformation(additionalInfo)
            )
        )
    { }
    
    public UpdateClinicCommand(int clinicId, UpdateClinicRequest request)
        : this(
            clinicId,
            new ClinicName(request.Name),
            new Address(
                new City(request.City),
                new Street(request.Street),
                new StreetNumber(request.StreetNumber),
                new AddressAdditionalInformation(request.AdditionalInfo ?? string.Empty)
            )
        )
    { }
}

public class UpdateClinicCommandHandler : IRequestHandler<UpdateClinicCommand>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateClinicCommandHandler(IClinicRepository clinicRepository, IUnitOfWork unitOfWork)
    {
        _clinicRepository = clinicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        clinic.Update(request.Name, request.Address);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}