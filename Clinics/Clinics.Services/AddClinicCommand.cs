using Clinics.Domain.ClinicAggregate;
using MediatR;
using Shared.Services;

namespace Clinics.Services;

public record AddClinicCommand(ClinicName Name, Address Address) : IRequest
{
    public AddClinicCommand(string name, string city, string street, string streetNumber, string additionalInfo)
        : this(
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

public class AddClinicCommandHandler : IRequestHandler<AddClinicCommand>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddClinicCommandHandler(IClinicRepository clinicRepository, IUnitOfWork unitOfWork)
    {
        _clinicRepository = clinicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = new Clinic(request.Name, request.Address);

        await _clinicRepository.AddAsync(clinic, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}