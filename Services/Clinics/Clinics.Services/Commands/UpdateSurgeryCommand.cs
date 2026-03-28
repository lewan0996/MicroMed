using Clinics.Contracts.Dto;
using Clinics.Contracts.Events;
using Clinics.Domain.ClinicAggregate;
using Clinics.Services.Repositories;
using MassTransit;
using MediatR;
using Shared.Services;

namespace Clinics.Services.Commands;

public record UpdateSurgeryCommand(Guid ClinicId, Guid SurgeryId, SurgeryInfo SurgeryInfo, IReadOnlyList<Guid> EquipmentIds) : IRequest
{
    public UpdateSurgeryCommand(Guid clinicId, Guid surgeryId, string number, string floor, IReadOnlyList<Guid> equipmentIds)
        : this(clinicId, surgeryId, new SurgeryInfo(new SurgeryNumber(number), new SurgeryFloor(floor)), equipmentIds) { }
    
    public UpdateSurgeryCommand(Guid clinicId, Guid surgeryId, UpdateSurgeryRequest request)
        : this(clinicId, surgeryId, new SurgeryInfo(new SurgeryNumber(request.Number), new SurgeryFloor(request.Floor)), request.EquipmentIds) { }
}

public class UpdateSurgeryCommandHandler(
    IClinicRepository clinicRepository,
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<UpdateSurgeryCommand>
{
    public async Task Handle(UpdateSurgeryCommand request, CancellationToken cancellationToken)
    {
        var clinic = await clinicRepository.GetAsync(request.ClinicId, cancellationToken);

        var equipment = await equipmentRepository.GetEquipmentAsync(request.EquipmentIds, cancellationToken);

        clinic.UpdateSurgery(request.SurgeryId, request.SurgeryInfo, equipment);

        await publishEndpoint.Publish(GetEvent(), cancellationToken: cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        SurgeryUpdatedEvent GetEvent() => new(request.SurgeryId, request.SurgeryInfo.Floor.Value, request.SurgeryInfo.Number.Value);
    }
}