using Clinics.Contracts;
using Clinics.Services.Repositories;
using MassTransit;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record RemoveSurgeryCommand(int ClinicId, int SurgeryId) : IRequest;

public class RemoveSurgeryCommandHandler : IRequestHandler<RemoveSurgeryCommand>
{
    private readonly IClinicRepository _clinicRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;

    public RemoveSurgeryCommandHandler(IClinicRepository clinicRepository, IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint)
    {
        _clinicRepository = clinicRepository;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(RemoveSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await _clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        clinic.RemoveSurgery(request.SurgeryId);

        await _publishEndpoint.Publish(GetEvent(), cancellationToken: cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        SurgeryRemovedEvent GetEvent() => new(request.SurgeryId);
    }
}