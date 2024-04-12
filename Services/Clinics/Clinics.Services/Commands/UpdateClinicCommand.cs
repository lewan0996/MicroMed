using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record UpdateClinicCommand(Guid ClinicId, ClinicName Name, Address Address) : IRequest
{
    public UpdateClinicCommand(Guid clinicId, string name, string city, string street, string streetNumber, string additionalInfo)
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