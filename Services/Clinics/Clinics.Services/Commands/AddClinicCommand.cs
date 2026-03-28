using Clinics.Contracts.Dto;
using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record AddClinicCommand(ClinicName Name, Address Address) : IRequest<Guid>
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
    
    public AddClinicCommand(AddClinicRequest request)
        : this(
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

public class AddClinicCommandHandler(IClinicRepository clinicRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<AddClinicCommand, Guid>
{
    public async Task<Guid> Handle(AddClinicCommand request, CancellationToken cancellationToken)
    {
        var clinic = new Clinic(request.Name, request.Address);

        await clinicRepository.AddAsync(clinic, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return clinic.Id;
    }
}