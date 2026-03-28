using Clinics.Contracts.Events;
using Clinics.Services.Repositories;
using MassTransit;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record RemoveSurgeryCommand(Guid ClinicId, Guid SurgeryId) : IRequest;

public class RemoveSurgeryCommandHandler(
    IClinicRepository clinicRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<RemoveSurgeryCommand>
{
    public async Task Handle(RemoveSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        clinic.RemoveSurgery(request.SurgeryId);

        await publishEndpoint.Publish(GetEvent(), cancellationToken: cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        SurgeryRemovedEvent GetEvent() => new(request.SurgeryId);
    }
}